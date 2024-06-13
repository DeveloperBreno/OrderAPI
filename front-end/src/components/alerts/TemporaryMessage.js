import './TemporaryMessage.css';

const TemporaryMessage = ({ message}) => {
  return (
    <div className={`temporary-message show`}>
      {message}
    </div>
  );
};

export default TemporaryMessage;