import React, { useEffect, useState } from 'react';
import ProductList from './components/partials/ProductList';
import Cart from './components/partials/Cart';
import Login from './components/partials/Login';
import Register from './components/partials/Register';
import Lojas from './components/partials/Lojas';

function App() {

  const [pathParam, setPathParam] = useState('');

  useEffect(() => {
    const path = window.location.pathname;
    const param = path.substring(path.lastIndexOf('/') + 1);
    setPathParam(param);
  }, []);

  const [lojas, setLojas] = useState([
    {
      id: 1,
      name: 'Eli docinhos',
      url: 'eli-docinhos',
      endereco: 'Rua 1, 234',
      disponivelAgora: true,
      image: 'https://images-na.ssl-images-amazon.com/images/I/61187631-L._SL1500_.jpg',
    },
    {
      id: 2,
      name: 'Adega x',
      url: 'adega-x',
      endereco: 'Rua x, 14',
      disponivelAgora: true,
      image: 'https://images-na.ssl-images-amazon.com/images/I/61187631-L._SL1500_.jpg',
    },
    {
      id: 3,
      name: 'Restaurante bad',
      url: 'restaurante-bad',
      endereco: 'Av. xyz, 501',
      disponivelAgora: false,
      image: 'https://images-na.ssl-images-amazon.com/images/I/61187631-L._SL1500_.jpg',
    },
  ]);

  const [products, setProducts] = useState([
    {
      id: 1,
      name: 'Marmita de frango',
      price: 18.99,
      currentPrice: 9.99,
      quantity: 0,
      image: 'https://images-na.ssl-images-amazon.com/images/I/61187631-L._SL1500_.jpg',
      stock: 6
    },
    {
      id: 2,
      name: 'Marmita de omelete',
      price: 18.99,
      currentPrice: 12.99,
      quantity: 0,
      image: 'https://images-na.ssl-images-amazon.com/images/I/61187631-L._SL1500_.jpg',
      stock: 6

    },
    {
      id: 3,
      name: 'Coca-cola (600ml)',
      price: 6.99,
      currentPrice: 12.99,
      quantity: 0,
      image: 'https://images-na.ssl-images-amazon.com/images/I/61187631-L._SL1500_.jpg',
      stock: 6
    },
  ]);

const setLoja = (urlLoja) => {

}

  const [cartItems, setCartItems] = useState([]);
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  const handleAddToCart = (product) => {
    const existe = cartItems.filter(o => o.id === product.id);
    if (existe.length > 0) {
      product.quantity = existe[0].quantity + 1;

      const newItems = cartItems.filter(o => o.id != product.id);
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

  const handleLogin = (cpf, password) => {
    // Implemente a lógica de autenticação aqui
    setIsLoggedIn(true);
  };

  const handleRegister = (address, number, cep, complement) => {
    // Implemente a lógica de cadastro aqui
  };

  return (
    <div>

      <>
        <div className='container'>
          <div className='row'>
            <div className='col-md-10'>

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
         <Lojas lojas={lojas} setLoja={setLoja} />
        </>
      )}

    </div>
  );
}

export default App;