import React from 'react';
import { Button } from 'react-bootstrap';

const Cart = ({ cartItems, onRemoveFromCart }) => {
  return (
    <div>
      <h2>Carrinho</h2>
      {cartItems.length === 0 ? (
        <p>Seu carrinho está vazio.</p>
      ) : (
        <table className="table table-striped table-bordered table-hover">
          <thead className="thead-dark">
            <tr>
              <th>Produto</th>
              <th>Preço</th>
              <th>Preço Atual</th>
              <th>Quantidade</th>
              <th>Ação</th>
            </tr>
          </thead>
          <tbody>
            {cartItems.map((item) => (
              <tr key={item.id}>
                <td>{item.name}</td>
                <td>{item.price.toFixed(2)}</td>
                <td>{item.currentPrice.toFixed(2)}</td>
                <td>{item.quantity}</td>
                <td>
                  <Button className='btn' variant="danger" onClick={() => onRemoveFromCart(item.id)}>
                    Remover
                  </Button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default Cart;
