import React, { useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';

const Login = ({ onLogin }) => {
  const [cpf, setCpf] = useState('');
  const [password, setPassword] = useState('');
  const [showLoginModal, setShowLoginModal] = useState(false);

  const handleSubmit = (e) => {
    e.preventDefault();
    onLogin(cpf, password);
    setShowLoginModal(false); // Fechar o modal após o login
  };

  const toggleLoginModal = () => {
    setShowLoginModal(!showLoginModal);
  };

  return (
    <div className="container">
      <div className="row">
        <div className="col-md-6">

          <a
            href="#"
            onClick={toggleLoginModal}
            className="mt-5 text-primary"
            style={{ textDecoration: 'underline', cursor: 'pointer' }}
          >
            Entrar
          </a>

          <Modal show={showLoginModal} onHide={toggleLoginModal}>
            <Modal.Header closeButton>
              <Modal.Title>Login</Modal.Title>
            </Modal.Header>
            <Modal.Body>
              <Form onSubmit={handleSubmit}>
                <Form.Group controlId="formCpf">
                  <Form.Label>CPF:</Form.Label>
                  <Form.Control
                    type="text"
                    value={cpf}
                    onChange={(e) => setCpf(e.target.value)}
                  />
                </Form.Group>

                <Form.Group controlId="formPassword">
                  <Form.Label>Senha:</Form.Label>
                  <Form.Control
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                  />
                </Form.Group>

                <Button variant="primary" type="submit" className="mt-2">
                  Entrar
                </Button>
              </Form>
            </Modal.Body>
          </Modal>
        </div>
      </div>
    </div>
  );
};

export default Login;
