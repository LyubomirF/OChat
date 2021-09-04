import * as signalR from "@microsoft/signalr";

class TestConection {
    constructor() {
        this.connection = new signalR.HubConnectionBuilder()
        .configureLogging(signalR.LogLevel.Debug)
        .withUrl("http://localhost:50241/chat", {
            accessTokenFactory: () => "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJjY2U2YTk2My1iMmI5LTQ1MTktYTJlNC05NjI2ZTczMGIzMmQiLCJ1bmlxdWVfbmFtZSI6Imx1Ym9GQGdtYWlsLmNvbSIsIm5iZiI6MTYyNzc2NzIxNywiZXhwIjoxNjI3Nzc0NDE2LCJpYXQiOjE2Mjc3NjcyMTd9.XDLY47PBaWJVLref8OuiZGU71PnYKw_S6sDJCgbFO2k",
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
        })
        .build();   
    }

    async startConnection() {
        try {
            await this.connection.start();
            console.log("SignalR Connected.");
        } catch (err) {
            console.log(err);
            setTimeout(this.startConnection, 5000);
        }
    }

    registerReceiveMessage() {
        this.connection.on("ReceiveMessage", (message) => {
            console.log("Received message:" + message);
        });
    }

    async sendMessage(message) {
        try {
            await this.connection.invoke("SendMessage", "hello");
        } catch (err) {
            console.error(err);
        }        
    }
}

const testConection = new TestConection();
testConection.startConnection();
export default testConection;
