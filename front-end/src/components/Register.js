import React, { useState } from 'react';

const Register = ({ onRegister }) => {
  const [address, setAddress] = useState('');
  const [number, setNumber] = useState('');
  const [cep, setCep] = useState('');
  const [complement, setComplement] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    onRegister(address, number, cep, complement);
  };

  return (
    <div>
      <h2>Cadastro</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="address">Endereço:</label>
          <input
            type="text"
            id="address"
            value={address}
            onChange={(e) => setAddress(e.target.value)}
          />
        </div>
        <div>
          <label htmlFor="number">Número:</label>
          <input
            type="text"
            id="number"
            value={number}
            onChange={(e) => setNumber(e.target.value)}
          />
        </div>
        <div>
          <label htmlFor="cep">CEP:</label>
          <input
            type="text"
            id="cep"
            value={cep}
            onChange={(e) => setCep(e.target.value)}
          />
        </div>
        <div>
          <label htmlFor="complement">Complemento:</label>
          <input
            type="text"
            id="complement"
            value={complement}
            onChange={(e) => setComplement(e.target.value)}
          />
        </div>
        <button type="submit">Cadastrar</button>
      </form>
    </div>
  );
};

export default Register;
