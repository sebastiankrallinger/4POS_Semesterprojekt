document.addEventListener('DOMContentLoaded', function() {
    // Füge den Event Listener erst hinzu, wenn das DOM vollständig geladen ist
    document.getElementById('user').addEventListener('submit', function(event) {
        event.preventDefault();
        var username = document.getElementById("username").value;
        var password = document.getElementById("password").value;

        var data = {
            username: username,
            password: password
        };
        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/app/user', true);
        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.onreadystatechange = function() {
            if (xhr.readyState === 4 && xhr.status === 201) {
                console.log('User saved');
                window.location.href = '/app/mainPage';
            }
        };
        xhr.send(JSON.stringify(data));
    });
});