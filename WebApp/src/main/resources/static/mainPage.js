let active_chat = null;
let previous_chat = null;
let username = null;

let socket = new WebSocket("ws://localhost:8080/ws");

socket.onopen = function(event) {
    console.log("Connected to WebSocket server.");
};

socket.onmessage = function(event) {
    getChats();
};

socket.onclose = function(event) {
    console.log("Disconnected from WebSocket server.");
};

function sendMessage(message) {
    socket.send(message);
}

document.addEventListener("DOMContentLoaded", function() {
    username = localStorage.getItem('username');
    console.log("username: " + username);
    getChats();
});

function getChats() {
    getUserId()
        .then(userId => {
            //console.log(userId);
            return fetch(`/app/users/${userId}/chats`)
        })
        .then(response => response.json())
        .then(chats => {
            //console.log("Chats erhalten:", chats);
            if (!chats){
                const chatListElement = document.getElementById('chatList');
                const noChatElement = document.createElement('p');
                noChatElement.textContent = 'Keine Chats verfügbar.';
                chatListElement.appendChild(noChatElement);
                return;
            }
            showChats(chats);
        })
        .catch(error => {
            console.error('Fehler beim Abrufen der Chats:', error);
        });
}

function showChats(chats){
    //console.log(chats)
    const chatListElement = document.getElementById('chatList');
    chatListElement.innerHTML = '';

    const headerElement = document.createElement('h2');

    headerElement.textContent = 'Deine Chats - ' + username;
    chatListElement.appendChild(headerElement);

    chats.forEach(chat => {
        //console.log(chat.bezeichnung + " - " + active_chat)
        if (active_chat != null && active_chat.bezeichnung == chat.bezeichnung){
            active_chat = chat;
            showMessages(active_chat.messages);
        }

        const chatButton = document.createElement('button');

        chatButton.textContent = chat.bezeichnung; // Bezeichnung des Chats
        if (chat.newMsg === true){
            chatButton.style.fontWeight = 'bold';
            chatButton.style.backgroundColor = '#FFD700';
        }
        chatButton.classList.add('chat-button');

        chatButton.addEventListener('click', () => {
            active_chat = chat;
            let btn = document.getElementById('btnMsg');
            let txtBox = document.getElementById('txtMsg');
            if (btn){
                btn.style.visibility = 'visible';
                txtBox.style.visibility = 'visible';
            }
            chatButton.style.fontWeight = 'normal';
            chatButton.style.backgroundColor = 'lightgrey';
            if (previous_chat != null && chatButton != previous_chat){
                previous_chat.style.backgroundColor = '';
            }
            previous_chat = chatButton;
            updateStatus(active_chat);
            showMessages(active_chat.messages);
        });
        chatListElement.appendChild(chatButton);
    });
}

function showMessages(messages) {
    //console.log(active_chat)
    const messageListElement = document.getElementById('messageList');
    messageListElement.innerHTML = '';

    //console.log(messages)
    if (messages != null) {
        messages.forEach(message => {
            if (message.receiver == false) {
                const messageElement = document.createElement('div');
                messageElement.textContent = message.message;
                messageListElement.appendChild(messageElement);
            }else if (message.receiver == true){
                const messageElement = document.createElement('div');
                messageElement.textContent = message.message;
                messageElement.className = "msgReceived";
                messageListElement.appendChild(messageElement);
            }

        });
    }
}

async function getUserId() {
    return fetch(`/app/users`)
        .then(response => response.json())
        .then(users => {
            for (let user of users) {
                if (user.username === username) {
                    return user.id;
                }
            }
            throw new Error('Benutzer nicht gefunden');
        });
}

function addChat(){
    let receiver
    let chatName

    getUserId()
        .then(userId => {
            receiver = prompt('Reveiver: ')
            chatName = prompt('Chat-Name:');
            if (chatName && receiver) {
                return fetch(`/app/addChat?userId=${userId}&chatName=${encodeURIComponent(chatName)}&receiver=${receiver}`, {
                    method: 'POST'
                });
            }
        })
        .then(response => {
            if (response.ok) {
                sendMessage("newChat");
                getChats();
            } else {
                throw new Error('Fehler beim Hinzufügen des Chats.');
            }
        })
        .catch(error => {
            console.error('Fehler beim Hinzufügen des Chats:', error);
        });
}

function updateStatus(chat){
    getUserId()
        .then(userId => {
            return fetch(`/app/updateStaus?id=${userId}&chatname=${chat.bezeichnung}`, {
                method: 'POST'
            });
        });
}

function addMsg(){
    let msg

    getUserId()
        .then(userId => {
            let inputField = document.getElementById('txtMsg');
            msg = inputField.value;
            inputField.value = '';
            if (msg) {
                return fetch(`/app/addMsg?id=${userId}&chatname=${active_chat.bezeichnung}&msg=${encodeURIComponent(msg)}&receiver=${active_chat.receiver}`, {
                    method: 'POST'
                });
            }
        })
        .then(response => {
            if (response.ok) {
                sendMessage("newMsg");
                getChats();
            } else {
                throw new Error('Fehler beim Hinzufügen der Msg.');
            }
        })
        .catch(error => {
            console.error('Fehler beim Hinzufügen der Msg:', error);
        })
}
