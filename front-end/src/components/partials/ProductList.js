import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCartShopping } from '@fortawesome/free-solid-svg-icons';
import ExibeValor from '../forms/ExibeValor';

const ProductList = ({ products, onAddToCart }) => {
  return (
    <div className="container">
      <h2 className="my-4">Produtos</h2>
      <div className="row">
        {products.map((product) => (
          <div key={product.id} className="col-md-4 mb-4">
            <div className="card h-100">
              <img src={product.image} className="card-img-top" alt={product.name} />
              <div className="card-body">
                <h5 className="card-title">{product.name}</h5>
                <p className="card-text">
                  Preço: <span className="font-weight-bold">
                    <ExibeValor preco={product.price} />
                    </span>
                </p>
                <p className="card-text">
                  Por apenas: <span className="font-weight-bold">
                    <ExibeValor preco={product.currentPrice} />
                    </span>
                </p>
                <p className="card-text">
                  Carrinho: <span className="font-weight-bold">{product.quantity}</span>
                </p>
                <p className="card-text">
                  Disponivel: <span className="font-weight-bold">{product.stock}</span>
                </p>
                {product.stock > product.quantity ? (
                  <button className="btn btn-primary" onClick={() => onAddToCart(product)}>
                    <FontAwesomeIcon icon={faCartShopping} />
                    <span style={{ marginLeft: '0.5rem' }}>Adicionar ao Carrinho</span>
                  </button>
                ) : (
                  <>
                    <div className="alert alert-warning" role="alert">
                      Acabou!
                    </div>
                  </>
                )}
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default ProductList;