import React, { Component } from 'react';
import * as signalR from '@aspnet/signalr-client';
import { HubConnection } from '@aspnet/signalr-client';

var ChatServerUrl = "http://localhost:7878/chat";
var ChatUrl = ChatServerUrl + "";
const numberOfRequest = 50;

class Chat extends Component {
  constructor(props) {
    super(props);
    
    this.state = {
      nick: '',
      message: '',
      messages: [],
      hubConnection: null,
    };
  }

  sendMultipleRequest = () => {
    for (let i = 0; i < numberOfRequest; i++) {
      this.state.hubConnection
        .invoke('sendSingle', this.state.nick, "Single - " + i)
        .catch(err => console.error(err));
    }
  
    this.setState({message: ''});      
  };

  sendSingleRequest = () => {
    this.state.hubConnection
      .invoke('sendArray', this.state.nick, "Array", numberOfRequest)
      .catch(err => console.error(err));
    
    this.setState({message: ''});      
  }

  componentDidMount () {
    const nick = window.prompt('Your name:', 'John');
    let hubConnection = new  HubConnection(ChatUrl, { transport: signalR.TransportType.LongPolling });

    this.setState({ hubConnection, nick }, () => {
      this.state.hubConnection
        .start()
        .then(() => console.log('Connection started!'))
        .catch(err => console.log('Error while establishing connection :('));

      this.state.hubConnection.on('sendToAll', (nick, receivedMessage) => {
        const text = `${nick}: ${receivedMessage}`;
        const messages = this.state.messages.concat([text]);
        this.setState({ messages });
      });
    });
  }

  render() {
    return (
        <div>
          <br />
          <input
            type="text"
            value={this.state.message}
            onChange={e => this.setState({ message: e.target.value })}
          />
    
          <button onClick={this.sendMultipleRequest}>Send Multiple Request</button>

          <button onClick={this.sendSingleRequest}>Send Single Request</button>
    
          <div>
            {this.state.messages.map((message, index) => (
              <span style={{display: 'block'}} key={index}> {message} </span>
            ))}
          </div>
        </div>
      );
  }
}

export default Chat;