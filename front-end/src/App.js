import React, { useEffect, useState } from 'react';
import ProductList from './components/partials/ProductList';
import Cart from './components/partials/Cart';
import Login from './components/partials/Login';
import Register from './components/partials/Register';
import Lojas from './components/partials/Lojas';
import TemporaryMessage from './components/alerts/TemporaryMessage';
import * as signalR from '@microsoft/signalr';

function App() {
  const [connection, setConnection] = useState(null);
  const [messages, setMessages] = useState([]);
  const [showMessage, setShowMessage] = useState(false);
  const [messageText, setMessageText] = useState('');
  const [pathParam, setPathParam] = useState('');
  const [lojas] = useState([
    {
      id: 1,
      name: 'Eli docinhos',
      url: 'eli-docinhos',
      endereco: 'Rua 1, 234',
      disponivelAgora: true,
      image: 'https://via.placeholder.com/300x300',
    },
    {
      id: 2,
      name: 'Adega x',
      url: 'adega-x',
      endereco: 'Rua x, 14',
      disponivelAgora: true,
      image: 'https://via.placeholder.com/300x300',
    },
    {
      id: 3,
      name: 'Restaurante bad',
      url: 'restaurante-bad',
      endereco: 'Av. xyz, 501',
      disponivelAgora: false,
      image: 'https://via.placeholder.com/300x300',
    }

  ]); // Defina suas lojas aqui
  const [products, setProducts] = useState([
    {
      id: 1,
      name: 'Marmita de frango',
      price: 18.99,
      currentPrice: 9.99,
      quantity: 0,
      image: 'https://via.placeholder.com/300x300',
      stock: 6
    },
    {
      id: 2,
      name: 'Marmita de omelete',
      price: 18.99,
      currentPrice: 12.99,
      quantity: 0,
      image: 'https://via.placeholder.com/300x300',
      stock: 6

    },
    {
      id: 3,
      name: 'Coca-cola (600ml)',
      price: 6.99,
      currentPrice: 12.99,
      quantity: 0,
      image: 'https://via.placeholder.com/300x300',
      stock: 6
    }

  ]); // Defina seus produtos aqui
  const [cartItems, setCartItems] = useState([]);
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  useEffect(() => {
    const serverUrl = 'http://localhost:5081'; // Substitua pelo endereço do seu servidor SignalR

    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${serverUrl}/chatHub`)
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection) {
      connection.on('ReceiveMessage', (message) => {
        setMessages(prevMessages => [...prevMessages, message]);
      });

      connection.start()
        .then(() => console.log('Conectado ao servidor SignalR'))
        .catch((error) => console.error(`Erro ao conectar ao servidor SignalR: ${error}`));

      return () => {
        connection.stop()
          .then(() => console.log('Conexão SignalR encerrada'))
          .catch((error) => console.error(`Erro ao encerrar conexão SignalR: ${error}`));
      };
    }
  }, [connection]);

  const sendMessage = async () => {
    try {
      await connection.invoke('SendMessage', 'ReactNativeClient', 'Olá do cliente React Native!');
    } catch (error) {
      console.error(`Erro ao enviar mensagem para o servidor SignalR: ${error}`);
    }
  };

  const notificar = (message) => {
    setMessageText(message);
    setShowMessage(true);
    setTimeout(() => setShowMessage(false), 5000);
  };

  const onSelectLoja = (loja) => {
    if (loja.disponivelAgora) {
      setPathParam(loja.url);
      window.location.href = `/${loja.url}`;
    } else {
      notificar(`A loja ${loja.name} está indisponível no momento.`);
    }
  };

  const handleAddToCart = (product) => {
    const existe = cartItems.find(o => o.id === product.id);

    if (existe) {
      product.quantity = existe.quantity + 1;
      setCartItems(cartItems.map(item => (item.id === product.id ? product : item)));
    } else {
      product.quantity = 1;
      setCartItems([...cartItems, product]);
    }
  };

  const handleRemoveFromCart = (productId) => {
    setCartItems(cartItems.filter(item => item.id !== productId));
    setProducts(products.map(product => {
      if (product.id === productId) product.quantity = 0;
      return product;
    }));
  };

  const handleLogin = async (email, password) => {
    try {
      const response = await fetch("http://localhost:5081/v1/User/Token", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({ email, password })
      });

      const data = await response.json();
      console.log(data);

      setIsLoggedIn(true);
    } catch (error) {
      console.error('Erro:', error);
    }
  };

  const handleRegister = (address, number, cep, complement) => {
    // Implemente a lógica de cadastro aqui
  };

  return (
    <div>
      {showMessage && <TemporaryMessage message={messageText} />}

      <div style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
        <p>Messages:</p>
        {messages.map((msg, index) => (
          <p key={index}>{msg}</p>
        ))}
        <button onClick={sendMessage}>Enviar Mensagem</button>
      </div>

      <div className='container'>
        <div className='row'>
          <div className='col-md-2'>
            {pathParam === '' ? (
              <div className='col-md-1'></div>
            ) : (
              <div>
                <p
                  href="#"
                  onClick={() => setPathParam('')}
                  className="mt-5 text-primary"
                  style={{ textDecoration: 'underline', cursor: 'pointer' }}
                >
                  Lojas
                </p>
              </div>
            )}
          </div>

          <div className='col-md-8'></div>

          {isLoggedIn ? (
            <div className='col-md-1'>
              Olá, usuário
            </div>
          ) : (
            <>
              <div className='col-md-1' style={{ textAlign: 'right' }}>
                <Register onRegister={handleRegister} />
              </div>
              <div className='col-md-1' style={{ textAlign: 'right' }}>
                <Login onLogin={handleLogin} />
              </div>
            </>
          )}
        </div>
      </div>

      {pathParam.length > 0 ? (
        <>
          <Cart cartItems={cartItems} onRemoveFromCart={handleRemoveFromCart} />
          <ProductList products={products} onAddToCart={handleAddToCart} />
        </>
      ) : (
        <Lojas lojas={lojas} onSelectLoja={onSelectLoja} />
      )}
    </div>
  );
}

export default App;
