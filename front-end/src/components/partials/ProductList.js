import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';

const ProductList = ({ products, onAddToCart }) => {
  return (
    <div className="container">
      <h2 className="my-4">Produtos</h2>
      <div className="row">
        {products.map((product) => (
          <div key={product.id} className="col-md-4 mb-4">
            <div className="card h-100">
              <div className="card-body">
                <h5 className="card-title">{product.name}</h5>
                <p className="card-text">
                  Preço: <span className="font-weight-bold">{product.price.toFixed(2)}</span>
                </p>
                <p className="card-text">
                  Por apenas: <span className="font-weight-bold">{product.currentPrice.toFixed(2)}</span>
                </p>
                <p className="card-text">
                  Carrinho: <span className="font-weight-bold">{product.quantity}</span>
                </p>
                <p className="card-text">
                  Disponivel: <span className="font-weight-bold">{product.stock}</span>
                </p>

                {product.stock > product.quantity ? (
                  <button
                    className="btn btn-primary"
                    onClick={() => onAddToCart(product)}
                  >
                    Adicionar ao Carrinho
                  </button>
                ) : (
                  <>
                    <div class="alert alert-warning" role="alert">
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
