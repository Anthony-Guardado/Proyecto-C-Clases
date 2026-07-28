
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventaMeCF.Models;

public class UnidadMedidasController : Controller
{
    private readonly InventaMeCFContext _context;

    public UnidadMedidasController(InventaMeCFContext context)
    {
        _context = context;
    }

    // GET: UNIDADMEDIDAS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.UnidadesMedida.ToListAsync());
    }

    // GET: UNIDADMEDIDAS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var unidadmedida = await _context.UnidadesMedida
            .FirstOrDefaultAsync(m => m.Id == id);
        if (unidadmedida == null)
        {
            return NotFound();
        }

        return View(unidadmedida);
    }

    // GET: UNIDADMEDIDAS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: UNIDADMEDIDAS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre")] UnidadMedida unidadmedida)
    {
        if (ModelState.IsValid)
        {
            _context.Add(unidadmedida);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(unidadmedida);
    }

    // GET: UNIDADMEDIDAS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var unidadmedida = await _context.UnidadesMedida.FindAsync(id);
        if (unidadmedida == null)
        {
            return NotFound();
        }
        return View(unidadmedida);
    }

    // POST: UNIDADMEDIDAS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre")] UnidadMedida unidadmedida)
    {
        if (id != unidadmedida.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(unidadmedida);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnidadMedidaExists(unidadmedida.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(unidadmedida);
    }

    // GET: UNIDADMEDIDAS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var unidadmedida = await _context.UnidadesMedida
            .FirstOrDefaultAsync(m => m.Id == id);
        if (unidadmedida == null)
        {
            return NotFound();
        }

        return View(unidadmedida);
    }

    // POST: UNIDADMEDIDAS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var unidadmedida = await _context.UnidadesMedida.FindAsync(id);
        if (unidadmedida != null)
        {
            _context.UnidadesMedida.Remove(unidadmedida);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool UnidadMedidaExists(int? id)
    {
        return _context.UnidadesMedida.Any(e => e.Id == id);
    }
}
