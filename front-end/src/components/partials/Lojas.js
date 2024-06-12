import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';

const Lojas = ({ lojas, setloja }) => {
  return (
    <div className="container">
      <h2 className="my-4">Lojas</h2>
      <div className="row">
        {lojas.map((loja) => (
          <div key={loja.id} className="col-md-4 mb-4">
            <div className="card h-100">
              <div className="card-body">
                <h5 className="card-title">{loja.name}</h5>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Lojas;
