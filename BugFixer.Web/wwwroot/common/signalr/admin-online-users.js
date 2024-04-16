
//#region create connection

let connection = new signalR.HubConnectionBuilder()
    .withAutomaticReconnect()
    .withUrl('/hubs/online-users')
    .build();

//#endregion

//#region get response from hub

connection.on("NewUserConnected", (userInfo) => {
    AddUserInfoToTable(userInfo);
});

connection.on("NewUserDisConnected", (userId) => {
    $(`#online-user-${userId}`).remove();
});

//#endregion

//#region start connection

function SuccessConnection() {
    console.log("successfully connected.");
    connection.invoke("GetAllConnectedUsers").then((usersList) => {
        usersList.forEach((user) => {
            AddUserInfoToTable(user);
        });
    });
}

function ErrorConnection() {
    console.log("error on connection.");
}

connection.start().then(SuccessConnection, ErrorConnection);

//#endregion

//#region other js

function AddUserInfoToTable(userInfo) {
    if ($(`#online-user-${userInfo.userId}`).length) {
        return;
    }

    let row = `
        <tr id="online-user-${userInfo.userId}">
            <td class="vertical-align-middle">
                ${userInfo.userId}
            </td>
            <td class="vertical-align-middle">
                ${userInfo.displayName}
            </td>
            <td class="vertical-align-middle">
                ${userInfo.connectedDate}
            </td>
        </tr>
    `;

    $("#OnlineUsersTableBody").prepend(row);
}

//#endregion