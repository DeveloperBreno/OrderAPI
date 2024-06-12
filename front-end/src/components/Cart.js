import React from 'react';

const Cart = ({ cartItems, onRemoveFromCart }) => {
  return (
    <div>
      <h2>Carrinho</h2>
      {cartItems.length === 0 ? (
        <p>Seu carrinho está vazio.</p>
      ) : (
        <ul>
          {cartItems.map((item) => (
            <li key={item.id}>
              <h3>{item.name}</h3>
              <p>Preço: {item.price.toFixed(2)}</p>
              <p>Preço Atual: {item.currentPrice.toFixed(2)}</p>
              <p>Quantidade: {item.quantity}</p>
              <button onClick={() => onRemoveFromCart(item.id)}>Remover</button>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default Cart;
