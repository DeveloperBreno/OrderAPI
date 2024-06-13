import React, { useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';

const Register = ({ onRegister }) => {
  const [showModal, setShowModal] = useState(false);
  const [address, setAddress] = useState('');
  const [number, setNumber] = useState('');
  const [cep, setCep] = useState('');
  const [complement, setComplement] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    onRegister(address, number, cep, complement);
    setShowModal(false); // Fechar o modal após o cadastro
  };

  const handleCloseModal = () => setShowModal(false);
  const handleShowModal = () => setShowModal(true);

  return (
    <>
      <div>
        <p
          href="#"
          onClick={handleShowModal}
          className="mt-5 text-primary"
          style={{ textDecoration: 'underline', cursor: 'pointer' }}
        >
          Cadastrar
        </p>
      </div>

      <Modal show={showModal} onHide={handleCloseModal}>
        <Modal.Header closeButton>
          <Modal.Title>Cadastro</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form onSubmit={handleSubmit}>
            <Form.Group controlId="formAddress">
              <Form.Label>Endereço:</Form.Label>
              <Form.Control
                type="text"
                value={address}
                onChange={(e) => setAddress(e.target.value)}
              />
            </Form.Group>
            <Form.Group controlId="formNumber">
              <Form.Label>Número:</Form.Label>
              <Form.Control
                type="text"
                value={number}
                onChange={(e) => setNumber(e.target.value)}
              />
            </Form.Group>
            <Form.Group controlId="formCep">
              <Form.Label>CEP:</Form.Label>
              <Form.Control
                type="text"
                value={cep}
                onChange={(e) => setCep(e.target.value)}
              />
            </Form.Group>
            <Form.Group controlId="formComplement">
              <Form.Label>Complemento:</Form.Label>
              <Form.Control
                type="text"
                value={complement}
                onChange={(e) => setComplement(e.target.value)}
              />
            </Form.Group>
            <Button variant="primary" type="submit" className="mt-2">
              Cadastrar
            </Button>
          </Form>
        </Modal.Body>
      </Modal>
    </>
  );
};

export default Register;
