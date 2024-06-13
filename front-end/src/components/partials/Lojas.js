import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { CSSTransition, TransitionGroup } from 'react-transition-group';

const Lojas = ({ lojas, onSelectLoja }) => {
  return (
    <div className="container">
      <h2 className="my-4">Lojas</h2>
      <div className="row">
        <TransitionGroup component={null}>
          {lojas.map((loja) => (
            <CSSTransition key={loja.id} timeout={1000} classNames="loja">
              <div className="col-md-4 mb-4">
                <div className="card h-100 pointer" onClick={() => onSelectLoja(loja)}>
                  <img src={loja.image} className="card-img-top" alt={loja.name} />
                  <div className="card-body">
                    <h5 className="card-title">{loja.name}</h5>
                    <p className="card-text">
                      {loja.disponivelAgora ? 'Disponível agora' : 'Indisponível no momento'}
                    </p>
                  </div>
                </div>
              </div>
            </CSSTransition>
          ))}
        </TransitionGroup>
      </div>
    </div>
  );
};

export default Lojas;