import React from 'react';

const ProductList = ({ products, onAddToCart }) => {
  return (
    <div>
      <h2>Produtos</h2>
      <ul>
        {products.map((product) => (
          <li key={product.id}>
            <h3>{product.name}</h3>
            <p>Preço: {product.price.toFixed(2)}</p>
            <p>Preço Atual: {product.currentPrice.toFixed(2)}</p>
            <p>Quantidade: {product.quantity}</p>
            <button onClick={() => onAddToCart(product)}>Adicionar ao Carrinho</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ProductList;
