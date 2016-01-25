import React, { PropTypes } from 'react';
import Message from './Message.jsx';

class MessageList extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      messages: [
        {id: 1, message: "hi there how are you?"},
        {id: 2, message: "Fine and yourself?"},
        {id: 3, message: "Hanging in there..."}
      ]
    };
  }

  render () {
    let messageNodes = this.state.messages.map((result) => {
      return (
        <Message key={result.id} message={result.message} />
      );
    });

    return (
      <div>{messageNodes}</div>
    )
  }
}

export default MessageList;
