function getChats() {
    //var userId = "65fafc00bf358250117a0dde";

    fetch(`/users/65fafc00bf358250117a0dde/chats`)
        .then(response => response.json())
        .then(chats => {
            chats.forEach(chat => {
                console.log(chat);
            });
        })
        .catch(error => console.error('Fehler beim Abrufen der Chats:', error));
}