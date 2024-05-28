# 4AHINF - POS - Semesterprojekt - Chatapp (Tiny WhatsApp) - Krallinger


## Softwarearchitektur

Die Chatapp, Tiny Whatsapp, besteht aus einer Client-Client-Server-Architektur. Es wurde ein Client als WPF-Anwendung und ein Client als WebApp implementiert. Diese kommunizieren mit dem Spring Boot Server. Die Benutzerdaten und Chats werden in einer MongoDB Datenbank gespeichert.

```mermaid
graph TD;
  A[WPF Client] <--> C[Spring Boot Server];
  B[Web Client] <--> C[Spring Boot Server];
  C[Spring Boot Server] <--> D[MongoDB];
```
<br>

## Beschreibung der Software

Die Chatapp bietet die Möglichkeit, dass Benutzer, über eine einfache Benutzeroberfläche, miteinander kommunizieren können. Der Benutzer kann entscheiden ob er die App im Web oder als WPF-Anwendung verwendet. Es ist möglich meherer Chats mit verschiedenen Benutzern zu haben und mit allen zu kommunizieren.

<br>

## Web-App - Funktionen

### Login - Web
![Login Page der Webseite](.\images\login_Web.png)<br>
<br>
Im Eingabefeld **"Username"** wird der Benutzernamen eingegeben.<br>
Im Eingabefeld **"Passwort"** wird das Passwort des Benutzers eingegeben.<br>
Beim klick auf den **"Login"** Button werden die Anmeldedaten überprüft und bei erfolgreicher Überprüfung wird der Benutzer auf die Hauptseite weitergeleitet.<br>
Bei ungültigen oder unvollständigen Benutzerdaten wird der Benutzer darauf hingewiesen, dass seine Daten nicht korrekt sind und es kann ein neuer Versuch gestartet werden.<br>
<br>
![Fehlerhafte Benutzerdaten](.\images\incorrectPassword_Web.png)<br>
<br>
![Unvollständige Benutzerdaten](.\images\incompleteInformation_Web.png)<br>
<br>
Wenn der eingegeben Benutzername noch nicht vorhanden ist wird ein neuer Benutzer erstellt.

### Mainpage - Web
![Mainpage der Webseite](.\images\incompleteInformation_Web.png)<br>
<br>

#### Chats - Web
Im linken, grauen Feld werden alle Chats eines Benutzers angezeigt.<br>
Bei klick auf einen Chat werden rechts die Nachrichten des Chats angezeigt.<br>
Bei klick auf den **"New Chat"** Button kann der Benutzer einen neuen Chat erstellen.<br>
Der Benutzer gibt zuerst den Benutzername des Empfängers und dann den Namen des Chats ein, sobald diese Eingabe erfolgt ist wird der Chat erstellt und im Chatfeld angezeigt.

#### Nachrichten - Web
Im rechten, weißen Feld werden alle Nachrichten eines Chats angezeigt, und in Echtzeit aktualisiert.<br>
Bei klick auf den **"New Message"** Button kann der Benutzer einen neue Nachricht senden.<br>
Der Benutzer gibt die Nachricht ein, sobald die Eingabe erfolgt ist wird die Nachricht zum Empfänger gesendet.<br>
Bei beiden wird die GUI aktualisiert und die neue Nachricht wird im Nachrichtenfeld angezeigt.


### WPF-App - Funktionen

### Login - WPF
![Login-Fenser der WPF-Anwendung](.\images\login_Web.png)<br>
<br>
Im Eingabefeld **"Username"** wird der Benutzernamen eingegeben.<br>
Im Eingabefeld **"Passwort"** wird das Passwort des Benutzers eingegeben.<br>
Beim klick auf den **"Login"** Button werden die Anmeldedaten überprüft und bei erfolgreicher Überprüfung wird der Benutzer auf das Hauptfenster weitergeleitet.<br>
Bei ungültigen oder unvollständigen Benutzerdaten wird der Benutzer darauf hingewiesen, dass seine Daten nicht korrekt sind und es kann ein neuer Versuch gestartet werden.<br>
<br>
![Fehlerhafte Benutzerdaten](.\images\incorrectPassword_Web.png)<br>
<br>
![Unvollständige Benutzerdaten](.\images\incompleteInformation_Web.png)<br>
<br>
Wenn der eingegeben Benutzername noch nicht vorhanden ist wird ein neuer Benutzer erstellt.

### Hauptfenster - WPF
![Mainpage der Webseite](.\images\incompleteInformation_Web.png)<br>
<br>

#### Chats - WPF
Im linken, grauen Feld werden alle Chats eines Benutzers angezeigt.<br>
Bei klick auf einen Chat werden rechts die Nachrichten des Chats angezeigt.<br>
Bei klick auf den **"New Chat"** Button kann der Benutzer einen neuen Chat erstellen.<br>
Der Benutzer gibt zuerst den Benutzername des Empfängers und dann den Namen des Chats ein, sobald diese Eingabe erfolgt ist wird der Chat erstellt und im Chatfeld angezeigt.

#### Nachrichten - WPF
Im rechten, weißen Feld werden alle Nachrichten eines Chats angezeigt, und in Echtzeit aktualisiert.<br>
Bei klick auf den **"New Message"** Button kann der Benutzer einen neue Nachricht senden.<br>
Der Benutzer gibt die Nachricht ein, sobald die Eingabe erfolgt ist wird die Nachricht zum Empfänger gesendet.<br>
Bei beiden wird die GUI aktualisiert und die neue Nachricht wird im Nachrichtenfeld angezeigt.


<br>

## API-Beschreibung
Die API wird durch einen Spring Boot Server basierend auf dem REST-Prinzip implementiert. Spring Boot ermöglicht es Clients mit dem Server, über GET und POST, zu kommunizieren und Daten auszutauschen. 

<br>

## Verwendung der API


<br>

## Diskussion der Ergebnisse


<br>

## Diagramme

<br>

## Quellenverzeichnis