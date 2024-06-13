function ExibeValor({preco}) {
    const precoEmTxt = preco.toFixed(2).replace('.', ',');
    return (
        <>
           R$ {precoEmTxt}
        </>
    )
}

export default ExibeValor;