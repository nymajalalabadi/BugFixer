
//#region create connection

let connection = new signalR.HubConnectionBuilder()
    .withAutomaticReconnect()
    .withUrl('/hubs/online-users')
    .build();

//#endregion

//#region get response from hub



//#endregion

//#region start connection

function SuccessConnection() {
    console.log("successfully connected.");
}

function ErrorConnection() {
    console.log("error on connection.");
}

connection.start().then(SuccessConnection, ErrorConnection);

//#endregion

//#region other js



//#endregion