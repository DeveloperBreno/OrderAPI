import React, { useState } from 'react';
import { Button, Card, Col, Row } from 'react-bootstrap';
import { CSSTransition, TransitionGroup } from 'react-transition-group';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrashAlt, faCartShopping, faChevronDown } from '@fortawesome/free-solid-svg-icons';
import ExibeValor from '../forms/ExibeValor';
import './Cart.css'; // Importando o CSS para as transições

const Cart = ({ cartItems, onRemoveFromCart }) => {
    const [isOpen, setIsOpen] = useState(false);

    const toggleCart = () => {
        setIsOpen(!isOpen);
    };

    const QuantidadeDeItensNoCarrinho = () => {
        let total = 0;
        for (var i = 0; i < cartItems.length; i++) {
            let produto = cartItems[i];
            total = total + produto.quantity;
        }

        return total; 
    };

    return (
        <div>
            {/* Botão fixo para abrir/fechar o carrinho */}


            <div className="cart-toggle btn btn-primary" onClick={toggleCart}>
                <FontAwesomeIcon icon={isOpen ? faChevronDown : faCartShopping} size="1x" />

                {!isOpen && (
                    <span className="badge text-bg-transparent">
                        {QuantidadeDeItensNoCarrinho()}
                    </span>
                )}
            </div>


            <div className={`cart-container ${isOpen ? 'open' : ''}`}>
                <div className="cart-header">
                    <h2 className="text-primary">
                        <FontAwesomeIcon icon={faCartShopping} /> 
                        <span style={{ marginLeft: '5px' }}> Carrinho </span>
                    </h2>
                </div>
                <div className="cart-content">
                    {cartItems.length === 0 ? (
                        <p>Seu carrinho está vazio.</p>
                    ) : (
                        <Row>
                            <TransitionGroup>
                                {cartItems.map((item) => (
                                    <CSSTransition key={item.id} timeout={300} classNames="cart-item">
                                        <Col xs={12} sm={6} md={4} lg={3} className="mb-3">
                                            <Card>
                                                <Card.Img
                                                    variant="top"
                                                    src={item.image}
                                                    style={{ height: '100px', objectFit: 'cover' }}
                                                />
                                                <Card.Body>
                                                    <Card.Title>{item.name}</Card.Title>
                                                    <Card.Text>
                                                        <strong>Preço:</strong> <ExibeValor preco={item.price} />
                                                    </Card.Text>
                                                    <Card.Text>
                                                        <strong>Por apenas:</strong> <ExibeValor preco={item.currentPrice} />
                                                    </Card.Text>
                                                    <Card.Text>
                                                        <strong>Quantidade:</strong> {item.quantity}
                                                    </Card.Text>
                                                    <Button
                                                        className="btn"
                                                        variant="light"
                                                        onClick={() => onRemoveFromCart(item.id)}
                                                    >
                                                        <FontAwesomeIcon icon={faTrashAlt} size="1x" color="#df433b" />
                                                    </Button>
                                                </Card.Body>
                                            </Card>
                                        </Col>
                                    </CSSTransition>
                                ))}
                            </TransitionGroup>
                        </Row>
                    )}
                </div>
            </div>
        </div>
    );
};

export default Cart;
