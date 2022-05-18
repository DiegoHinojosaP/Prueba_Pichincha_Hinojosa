using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ApiHinojosaPrueba.Datos;
using Web.ApiHinojosaPrueba.Modelos;
using Web.ApiHinojosaPrueba.Utilitario;

namespace Web.ApiHinojosaPrueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly BaseDatosContext _contexto;
        public ClienteController(BaseDatosContext contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        [Route("Clientes")]
        public async Task<ActionResult<List<Cliente>>> Consultar()
        {
            return await _contexto.Clientes.ToListAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Resultado> ConsultarId(int id)
        {
            Resultado oResultado = new();
            try
            {
                oResultado.Respuesta = await _contexto.Clientes.FindAsync(id);
                oResultado.Exito = true;
            }
            catch (Exception ex)
            {
                oResultado.Mensaje = "Se genero un error: " + ex.StackTrace;
            }

            return oResultado;
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Ingreso(Cliente cliente)
        {
            _contexto.Clientes.Add(cliente);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction("ConsultarId", new { id = cliente.CliIdCliente }, cliente);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Modificar(int id, Cliente cliente)
        {
            if (id != cliente.CliIdCliente)
            {
                return BadRequest();
            }

            _contexto.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync().ConfigureAwait(true);
            }
            catch (DbUpdateConcurrencyException)
            {
               
                if (!_contexto.Clientes.Any(e => e.CliIdCliente == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<Resultado> Eliminar(int id)
        {
            Resultado oResultado = new();
            try
            {
                var oCliente = await _contexto.Clientes.FindAsync(id);
                oResultado.Respuesta = _contexto.Clientes.Remove(oCliente);
                await _contexto.SaveChangesAsync();
                oResultado.Exito = true;
            }
            catch (Exception ex)
            {
                oResultado.Mensaje = "Error: " + ex.StackTrace;
            }

            return oResultado; 
        }

    }
}
