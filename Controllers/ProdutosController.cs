using Microsoft.AspNetCore.Mvc;
using WebApiMongoDB01.Models;
using WebApiMongoDB01.Services;

namespace WebApiMongoDB01.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly ProdutoService _produtoService;

    public ProdutosController(ProdutoService produtoService)
    {
        this._produtoService = produtoService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post(Produto produto)
    {
        await _produtoService.CreateAsync(produto);
        return Ok(produto);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get()
               => Ok(await _produtoService.GetAsync());

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Produto>> Get(string id)
    {
        var produto = await _produtoService.GetAsync(id);

        if (produto is null)
            return NotFound();

        return Ok(produto);
    }


    [HttpPut("{id:length(24)}")]
    [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put(string id, Produto updateProduto)
    {
        var produto = await _produtoService.GetAsync(id);

        if (produto is null)
            return NotFound();

        updateProduto.Id = produto.Id;

        await _produtoService.UpdateAsync(id, updateProduto);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(string id)
    {
        var produto = await _produtoService.GetAsync(id);

        if (produto is null)
            return NotFound();
        
        await _produtoService.RemoveAsync(id);

        return NoContent();
    }
}
