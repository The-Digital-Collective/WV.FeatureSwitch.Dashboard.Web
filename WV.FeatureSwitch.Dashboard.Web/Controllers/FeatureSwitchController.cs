using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WV.FeatureSwitch.Dashboard.BAL.Models;
using WV.FeatureSwitch.Dashboard.DAL.DBContext;
using WV.FeatureSwitch.Dashboard.DAL.Entities;
using WV.FeatureSwitch.Dashboard.Web.ViewModels;

namespace WV.FeatureSwitch.Dashboard.Web.Controllers
{
    public class FeatureSwitchController : Controller
    {
        private readonly FeatureSwitchDbContext _context;

        public FeatureSwitchController(FeatureSwitchDbContext context)
        {
            _context = context;
        }

        public IActionResult Index1()
        {
            FeatureSwitchViewModel featureSwitchViewModel = new FeatureSwitchViewModel()
            {
                FeatureModel = new List<FeatureModel>() { }
            };
            
            var items = _context.Features.ToList();

            foreach (var item in items)
            {
                featureSwitchViewModel.FeatureModel.Add(new FeatureModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Flag = item.Flag
                });
            }

            return View(featureSwitchViewModel);

            //List<FeatureViewModel> viewModel = new List<FeatureViewModel>();
            //var items = _context.Features.ToList();
            //foreach (var item in items)
            //{
            //    viewModel.Add(new FeatureViewModel
            //    {
            //        Id = item.Id,
            //        Name = item.Name,
            //        Flag = item.Flag
            //    });
            //}
            //return View(viewModel);
        }

        // GET: FeatureSwitch
        //public IActionResult oldIndex1()
        //{

        //    List<FeatureViewModel> viewModel = new List<FeatureViewModel>();
        //    var items = _context.Features.ToList();
        //    foreach (var item in items)
        //    {
        //        viewModel.Add(new FeatureViewModel
        //        {
        //            Id = item.Id,
        //            Name = item.Name,
        //            Flag = item.Flag
        //        });
        //    }
        //    return View(viewModel);           
        //}

        // GET: FeatureSwitch - working
        public async Task<IActionResult> Index()
        {
            return View(await _context.Features.ToListAsync());
        }

        // GET: FeatureSwitch/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var feature = await _context.Features
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (feature == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(feature);
        //}

        // GET: FeatureSwitch/Create
        public IActionResult AddOrEdit(int id=0)
        {
            if (id == 0)
            {
                return View(new Feature());
            }
            else
            {
                return View(_context.Features.Find(id));
            }
        }

        // POST: FeatureSwitch/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,Name,Flag")] Feature feature)
        {
            if (ModelState.IsValid)
            {
                if(feature.Id == 0)
                {
                    _context.Add(feature);
                }
                else
                {
                    _context.Update(feature);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(feature);
        }

        // GET: FeatureSwitch/Delete
        //public async Task<IActionResult> Delete(List<FeatureViewModel> viewModelList)
        //{
        //    List<Feature> feature = new List<Feature>();

        //    foreach (var item in viewModelList)
        //    {
        //        if (item.SelectedItem.Selected)
        //        {
        //            var selectedFeature = _context.Features.Find(item.Id);
        //            feature.Add(selectedFeature);
        //        }                
        //    }

        //    _context.Features.RemoveRange(feature);
        //    _context.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}

        // GET: FeatureSwitch/Delete/{id} - working
        public async Task<IActionResult> Delete(int? id)
        {
            var feature = await _context.Features.FindAsync(id);
            _context.Features.Remove(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: FeatureSwitch/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var feature = await _context.Features.FindAsync(id);
        //    if (feature == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(feature);
        //}

        // POST: FeatureSwitch/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Flag")] Feature feature)
        //{
        //    if (id != feature.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(feature);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!FeatureExists(feature.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(feature);
        //}

        // GET: FeatureSwitch/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var feature = await _context.Features
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (feature == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(feature);
        //}

        // POST: FeatureSwitch/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var feature = await _context.Features.FindAsync(id);
        //    _context.Features.Remove(feature);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool FeatureExists(int id)
        //{
        //    return _context.Features.Any(e => e.Id == id);
        //}
    }
}
