document.addEventListener("DOMContentLoaded", function() {
    getChats();
});

function getChats() {
    getUserId()
        .then(userId => {
            console.log(userId);
            return fetch(`/app/users/${userId}/chats`);
        })
        .then(response => response.json())
        .then(chats => {
            console.log(chats)
            showChats(chats);
        })
        .catch(error => {
            console.error('Fehler beim Abrufen der Chats:', error);
        });
}
function showChats(chats){
    const chatListElement = document.getElementById('chatList');
    chatListElement.innerHTML = '';

    const headerElement = document.createElement('h2');
    headerElement.textContent = 'Deine Chats';
    chatListElement.appendChild(headerElement);

    chats.forEach(chat => {
        const chatButton = document.createElement('button');
        chatButton.textContent = chat.bezeichnung; // Bezeichnung des Chats
        chatButton.classList.add('chat-button');
        chatButton.addEventListener('click', () => {
            showMessages(chat.messages);
        });
        chatListElement.appendChild(chatButton);
    });
}

function showMessages(messages) {
    const messageListElement = document.getElementById('messageList');
    messageListElement.innerHTML = '';
    console.log(messages)
    messages.forEach(message => {
        const messageElement = document.createElement('div');
        messageElement.textContent = message.message;
        messageListElement.appendChild(messageElement);
    });
}

function getUserId(){
    var username = getParameterByName('username');
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

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}