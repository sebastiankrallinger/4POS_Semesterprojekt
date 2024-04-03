document.addEventListener('DOMContentLoaded', function() {
    document.getElementById('user').addEventListener('submit', function(event) {
        event.preventDefault();
        let username = document.getElementById("username").value;
        let password = document.getElementById("password").value;

        let data = {
            username: username,
            password: password
        };
        let xhr = new XMLHttpRequest();
        xhr.open('POST', '/app/user', true);
        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.onreadystatechange = function() {
            if (xhr.readyState === 4 && xhr.status === 201) {
                console.log('User saved');
                document.getElementById("username-info").hidden = false;
                document.getElementById("username-info").innerText = "";
                window.location.href = '/app/mainPage';
            }else {
                console.log('Passwort Falsch')
                document.getElementById("username-info").hidden = true;
                document.getElementById("username-info").innerText = "Falsches Passwort - erneut versuchen";
            }
        };
        xhr.send(JSON.stringify(data));
    });
});