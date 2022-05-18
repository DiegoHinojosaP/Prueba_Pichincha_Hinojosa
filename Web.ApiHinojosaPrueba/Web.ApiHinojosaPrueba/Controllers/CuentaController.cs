using Microsoft.AspNetCore.Http;
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
    public class CuentaController : ControllerBase
    {
        private readonly BaseDatosContext _context;
        public CuentaController(BaseDatosContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cuenta>>> Consultar()
        {
            return await _context.Cuentas.ToListAsync();
        }

        // GET: api/Cuentas/5
        [HttpGet("{id}")]
        public async Task<Resultado> CosultarId(string id)
        {
            Resultado oResultado = new();
            try
            {
                oResultado.Respuesta = await _context.Cuentas.FindAsync(id);
                oResultado.Exito = true;
            }
            catch (Exception ex)
            {
                oResultado.Mensaje = "Error: " + ex.StackTrace;
            }
            return oResultado;
        }

        // PUT: api/Cuentas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Modificar(string id, Cuenta cuenta)
        {
            if (id != cuenta.CueNumero)
            {
                return BadRequest();
            }

            _context.Entry(cuenta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Cuentas.Any(e => e.CueNumero == cuenta.CueNumero))
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

        [HttpPost]
        public async Task<Resultado> Ingreso(Cuenta cuenta)
        {
            Resultado respuesta = new();
            _context.Cuentas.Add(cuenta);
            try
            {
                await _context.SaveChangesAsync();
                respuesta.Exito = true;
            }
            catch (DbUpdateException e)
            {
                respuesta.Exito = false;
                if (_context.Cuentas.Any(e => e.CueNumero == cuenta.CueNumero))
                {
                    respuesta.Mensaje = "La cuenta ya existe";
                }
                else
                {
                    respuesta.Mensaje = "Ocurrio un error: " + e.StackTrace;
                }
            }

            respuesta.Respuesta = CreatedAtAction("Consultar", new { id = cuenta.CueNumero }, cuenta);

            return respuesta;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Elminar(string id)
        {

            var pCuenta = await _context.Cuentas.FindAsync(id);
            if (pCuenta == null)
            {
                return NotFound();
            }

            _context.Cuentas.Remove(pCuenta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
