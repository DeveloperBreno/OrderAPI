import React from 'react';
import { Button } from 'react-bootstrap';
import { CSSTransition, TransitionGroup } from 'react-transition-group';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { faCartShopping } from '@fortawesome/free-solid-svg-icons';
import ExibeValor from '../forms/ExibeValor';
const Cart = ({ cartItems, onRemoveFromCart }) => {
  return (
    <div className="container mt-3">
      <h2><FontAwesomeIcon icon={faCartShopping} /> 
      <span style={{ marginLeft: '0.5rem' }}> Carrinho </span> </h2>

      <FontAwesomeIcon icon="fa-solid fa-shop" />
      {cartItems.length === 0 ? (
        <p>Seu carrinho está vazio.</p>
      ) : (
        <>
          <table className="table table-striped table-bordered table-hover">
            <thead className="thead-dark">
              <tr>
                <th>Foto</th>
                <th>Produto</th>
                <th>Preço</th>
                <th>Por apenas</th>
                <th>Quantidade</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              <TransitionGroup component={null}>
                {cartItems.map((item) => (
                  <CSSTransition key={item.id} timeout={300} classNames="cart-item">
                    <tr>
                      <td>
                      <img src={item.image} className="card-img-top" alt={item.name} 
                       style={{ width: '70px', height: '70px' }} />
                      </td>
                      <td>{item.name}</td>
                      <td><ExibeValor preco={item.price} /></td>
                      <td><ExibeValor preco={item.currentPrice} /></td>
                      <td>{item.quantity}</td>
                      <td>
                        <Button
                          className="btn"
                          variant="light"
                          onClick={() => onRemoveFromCart(item.id)}
                        >
                          <FontAwesomeIcon icon={faTrashAlt} size="1x" color="#df433b" />

                        </Button>
                      </td>
                    </tr>
                  </CSSTransition>
                ))}
              </TransitionGroup>
            </tbody>
          </table>

          <div>

          </div>

        </>

      )}
    </div>
  );
};

export default Cart;
