import React, { useState } from 'react';
import ProductList from './components/ProductList';
import Cart from './components/Cart';
import Login from './components/Login';
import Register from './components/Register';

function App() {
  const [products, setProducts] = useState([
    { id: 1, name: 'Produto 1', price: 10.99, currentPrice: 9.99, quantity: 5 },
    { id: 2, name: 'Produto 2', price: 15.99, currentPrice: 12.99, quantity: 3 },
    // Adicione mais produtos conforme necessário
  ]);

  const [cartItems, setCartItems] = useState([]);
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  const handleAddToCart = (product) => {
    setCartItems([...cartItems, product]);
  };

  const handleRemoveFromCart = (productId) => {
    setCartItems(cartItems.filter((item) => item.id !== productId));
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
      <ProductList products={products} onAddToCart={handleAddToCart} />
      <Cart cartItems={cartItems} onRemoveFromCart={handleRemoveFromCart} />
      <Login onLogin={handleLogin} />
      <Register onRegister={handleRegister} />
    </div>
  );
}

export default App;
