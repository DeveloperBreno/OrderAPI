import React, { useEffect, useState } from 'react';
import ProductList from './components/partials/ProductList';
import Cart from './components/partials/Cart';
import Login from './components/partials/Login';
import Register from './components/partials/Register';
import Lojas from './components/partials/Lojas';
import TemporaryMessage from './components/alerts/TemporaryMessage';

function App() {

  const [showMessage, setShowMessage] = useState(false);
  const [messageText, setMessageText] = useState('');

  const [pathParam, setPathParam] = useState('');

  useEffect(() => {
    const path = window.location.pathname;
    const param = path.substring(path.lastIndexOf('/') + 1);
    setPathParam(param);
  }, []);

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
    },
  ]);

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
    },
  ]);

  const notificar = (message) => {
    setMessageText(message);
    setShowMessage(true);
    setTimeout(() => {
      setShowMessage(false);
    }, 5000);
  };

  const onSelectLoja = (loja) => {
    if (loja.disponivelAgora) {
      setPathParam(loja.url);
      window.location.href = `/${loja.url}`;
    } else {
      notificar(`A loja ${loja.name} está indisponível no momento.`);
    }
  };

  const [cartItems, setCartItems] = useState([]);
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  const handleAddToCart = (product) => {
    const existe = cartItems.filter(o => o.id === product.id);
    if (existe.length > 0) {
      product.quantity = existe[0].quantity + 1;

      const newItems = cartItems.filter(o => o.id !== product.id);
      setCartItems([...newItems, product]);

    } else {
      product.quantity = 1;
      setCartItems([...cartItems, product]);
    }

  };

  const handleRemoveFromCart = (productId) => {
    setCartItems(cartItems.filter((item) => item.id !== productId));

    const produto = products.filter(o => o.id === productId)[0];
    produto.quantity = 0;

    const novaListaDeProduto = products.filter(o => o.id !== productId);

    setProducts([...novaListaDeProduto, produto]);
  };

  const handleLogin = async (cpf, password) => {
    // Implemente a lógica de autenticação aqui

    try {
      let headersList = {
        "User-Agent": "Thunder Client (https://www.thunderclient.com)",
        "accept": "*/*",
        "Content-Type": "application/json"
      }

      let bodyContent = JSON.stringify({
        "email": "developerbreno@gmail.com",
        "senha": "DEVBRE>no.257.DEVBRE",
        "celular": "string",
        "nascimento": "2024-06-13T00:33:09.644Z"
      });

      let response = await fetch("http://localhost:5000/User/Token", {
        method: "POST",
        body: bodyContent,
        headers: headersList,
        mode: 'cors', // Adicione esta linha
        credentials: 'include' // Adicione esta linha
      });

      let data = await response.text();
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

      <>
        <div className='container'>
          <div className='row'>



            <div className='col-md-2'>

              {pathParam === '' ? (
                <div className='col-md-1'>

                </div>
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

            <div className='col-md-8'>

            </div>
            {isLoggedIn ? (
              <div className='col-md-1'>
                Olá, usuario
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
      </>


      {pathParam.length > 0 ? (
        <>
          <Cart cartItems={cartItems} onRemoveFromCart={handleRemoveFromCart} />
          <ProductList products={products} onAddToCart={handleAddToCart} />
        </>
      ) : (
        <>
          <Lojas lojas={lojas} onSelectLoja={onSelectLoja} />
        </>
      )}

    </div>
  );
}

export default App;