import React, { useState } from 'react';
import agent from '../data/agent';
// Ažuriraj putanju prema tvom agentu

const ChatComponent: React.FC = () => {
  const [userMessage, setUserMessage] = useState('');
  const [chatHistory, setChatHistory] = useState<
    { role: string; content: string }[]
  >([]);

  const handleSendMessage = async () => {
    if (!userMessage) return;

    const newMessage = { role: 'user', content: userMessage };
    const updatedChatHistory = [...chatHistory, newMessage];

    setChatHistory(updatedChatHistory);
    setUserMessage('');

    try {
      const response = await agent.sendChatMessage(userMessage);
      const botMessage = { role: 'assistant', content: response };

      setChatHistory((prev) => [...prev, botMessage]);
    } catch (error) {
      console.error('Error sending message:', error);
    }
  };

  return (
    <div className="chat-container">
      <div className="chat-history">
        {chatHistory.map((msg, index) => (
          <div key={index} className={msg.role}>
            <strong>{msg.role === 'user' ? 'Ti' : 'ChatGPT'}:</strong>{' '}
            {msg.content}
          </div>
        ))}
      </div>
      <input
        type="text"
        value={userMessage}
        onChange={(e) => setUserMessage(e.target.value)}
        placeholder="Unesite poruku..."
      />
      <button onClick={handleSendMessage}>Pošalji</button>
    </div>
  );
};

export default ChatComponent;
