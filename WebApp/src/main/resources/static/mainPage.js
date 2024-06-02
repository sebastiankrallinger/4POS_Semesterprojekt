let active_chat = null;
let previous_chat = null;
let previous_date = null;
let username = null;

//Websocket Verwaltung
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

//Daten am Beginn laden
document.addEventListener("DOMContentLoaded", function() {
    username = localStorage.getItem('username');
    //console.log("username: " + username);
    getChats();
});

//Chats laden
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

//Chats anzeigen
function showChats(chats){
    //console.log(chats)
    const chatListElement = document.getElementById('chatList');
    chatListElement.innerHTML = '';

    const headerElement = document.createElement('h2');

    headerElement.className = "headChats";
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
            chatButton.style.backgroundColor = 'lightgreen';
        }
        chatButton.classList.add('chat-button');

        chatButton.addEventListener('click', async () => {
            active_chat = chat;
            let btn = document.getElementById('btnMsg');
            let txtBox = document.getElementById('txtMsg');
            if (btn) {
                btn.style.visibility = 'visible';
                txtBox.style.visibility = 'visible';
            }
            chatButton.style.fontWeight = 'normal';
            chatButton.style.backgroundColor = 'lightgrey';
            if (previous_chat != null && chatButton != previous_chat) {
                previous_chat.style.backgroundColor = '';
            }
            previous_chat = chatButton;
            await updateStatus(active_chat);
            showMessages(active_chat.messages);
        });
        chatListElement.appendChild(chatButton);
    });
}

//Nachrichten anzeigen
function showMessages(messages) {
    //console.log(active_chat)
    previous_date = null;
    const messageListElement = document.getElementById('messageList');
    messageListElement.innerHTML = '';

    //console.log(messages)
    if (messages != null) {
        messages.forEach(message => {
            let dateString = message.date;
            let [datePart, timePart] = dateString.split(' ');
            let [day, month, year] = datePart.split('-');
            let formattedDate = `${day}.${month}.${year}`;
            if (formattedDate != previous_date){
                previous_date = formattedDate;
                let dateElement = document.createElement('div');
                dateElement.textContent = formattedDate;
                dateElement.className = "date";
                messageListElement.appendChild(dateElement);
            }
            if (message.receiver == false) {
                let messageElement = document.createElement('div');
                let timeElement = document.createElement('div');
                timeElement.textContent = timePart;
                timeElement.className = "time";
                messageElement.textContent = message.message;
                messageElement.className = "msgSent";
                messageElement.appendChild(timeElement);
                messageListElement.appendChild(messageElement);
            } else if (message.receiver == true){
                let messageElement = document.createElement('div');
                let timeElement = document.createElement('div');
                timeElement.textContent = timePart;
                timeElement.className = "time";
                messageElement.textContent = message.message;
                messageElement.className = "msgReceived";
                messageElement.appendChild(timeElement);
                messageListElement.appendChild(messageElement);
            }
        });

        scrollToBottom();
    }
}

//immer zur neuesten Nachricht scrollen
function scrollToBottom() {
    const messageListElement = document.getElementById('messageList');
    messageListElement.scrollTop = messageListElement.scrollHeight;
}

//User ID abfragen
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

//Chat hinzufügen
function addChat(){
    let receiver;
    let chatName;

    getUserId()
        .then(userId => {
            receiver = prompt('Empfänger (Username) eingeben: ');
            chatName = prompt('Chat-Name eingeben:');
            if (chatName && receiver) {
                return fetch(`/app/addChat?userId=${userId}&chatName=${encodeURIComponent(chatName)}&receiver=${receiver}`, {
                    method: 'POST'
                });
            }else {
               throw new Error('Unvollständige Eingabe!')
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
            alert(error);
            console.error('Fehler beim Hinzufügen des Chats:', error);
        });
}

//Chatstatus aktualisieren
async function  updateStatus(chat){
    getUserId()
        .then(userId => {
            return fetch(`/app/updateStaus?id=${userId}&chatname=${chat.bezeichnung}`, {
                method: 'POST'
            });
        });
}

//Nachricht senden
function addMsg(){
    let msg;

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
        .then(async response => {
            if (response.ok) {
                sendMessage("newMsg");
                await updateStatus(active_chat);
                getChats();
            } else {
                throw new Error('Fehler beim Hinzufügen der Msg.');
            }
        })
        .catch(error => {
            console.error('Fehler beim Hinzufügen der Msg:', error);
        });
}

//'Enter' EventHandler
function handleEnter(event) {
    if (event.keyCode === 13) {
        event.preventDefault();
        addMsg();
    }
}
