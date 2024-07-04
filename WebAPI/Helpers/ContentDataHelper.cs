using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Objects;

namespace WebAPI.Helpers
{
    public class ContentDataHelper
    {
        private readonly Context _context;
        public ContentDataHelper(Context context) {
            _context = context;
        }

        public async Task<List<string>> GetAllHtmlNamesAsync()
        {
            List<Html> data = await _context.HTML.ToListAsync();
            List<string> names = new();
            foreach (Html item in data)
            {
                if (item.Name == null) {}
                else
                {
                    names.Add(item.Name);
                }
            }

            return names;
        }

        public async Task<string?> GetHtmlDataAsync(string name)
        {
            Html? data = await _context.HTML.FirstOrDefaultAsync(x => x.Name == name);
            if (data == null)
            {
                return null;
            }
            else
            {
                return data.Content;
            }
        }

        public async Task AddHtmlDataAsync(string name, string content)
        {
            Html? data = await _context.HTML.FirstOrDefaultAsync(x => x.Name == name);
            if(data == null)
            {
                await _context.HTML.AddAsync(new Html
                {
                    ID = Guid.NewGuid(),
                    Name = name,
                    Content = content
                });
            }
            else
            {
                data.Content = content;
                _context.HTML.Update(data);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AppendHtmlDataAsync(string name, string content)
        {
            Html? data = await _context.HTML.FirstOrDefaultAsync(x => x.Name == name);
            if (data == null)
            {
                await _context.HTML.AddAsync(new Html
                {
                    ID = Guid.NewGuid(),
                    Name = name,
                    Content = content
                });
            }
            else
            {
                data.Content += content;
                _context.HTML.Update(data);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveHtmlAsync(List<string?> names) 
        {
            foreach (string? name in names)
            {
                Html? data = await _context.HTML.FirstOrDefaultAsync(x => x.Name == name);
                if (data == null) { }
                else
                {
                    _context.HTML.Remove(data);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetAllCssNamesAsync()
        {
            List<Css> data = await _context.Css.ToListAsync();
            List<string> names = new();
            foreach (Css item in data)
            {
                if (item.Name == null) { }
                else
                {
                    names.Add(item.Name);
                }
            }

            return names;
        }

        public async Task<string?> GetCssDataAsync(string name)
        {
            Css? data = await _context.Css.FirstOrDefaultAsync(x => x.Name == name);
            if (data == null)
            {
                return null;
            }
            else
            {
                return data.Content;
            }
        }

        public async Task AddCssDataAsync(string name, string content)
        {
            Css? data = await _context.Css.FirstOrDefaultAsync(x => x.Name == name);
            if (data == null)
            {
                await _context.Css.AddAsync(new Css
                {
                    ID = Guid.NewGuid(),
                    Name = name,
                    Content = content
                });
            }
            else
            {
                data.Content = content;
                _context.Css.Update(data);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AppendCssDataAsync(string name, string content)
        {
            Css? data = await _context.Css.FirstOrDefaultAsync(x => x.Name == name);
            if (data == null)
            {
                await _context.Css.AddAsync(new Css
                {
                    ID = Guid.NewGuid(),
                    Name = name,
                    Content = content
                });
            }
            else
            {
                data.Content += content;
                _context.Css.Update(data);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveCssAsync(List<string?> names)
        {
            foreach (string? name in names)
            {
                Css? data = await _context.Css.FirstOrDefaultAsync(x => x.Name == name);
                if (data == null) { }
                else
                {
                    _context.Css.Remove(data);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetAllJavascriptNamesAsync()
        {
            List<Javascript> data = await _context.Javascript.ToListAsync();
            List<string> names = new();
            foreach (Javascript item in data)
            {
                if (item.Name == null) { }
                else
                {
                    names.Add(item.Name);
                }
            }

            return names;
        }

        public async Task<string?> GetJavascriptDataAsync(string name)
        {
            Javascript? data = await _context.Javascript.FirstOrDefaultAsync(x => x.Name == name);
            if (data == null)
            {
                return null;
            }
            else
            {
                return data.Content;
            }
        }

        public async Task AddJavascriptDataAsync(string name, string content)
        {
            Javascript? data = await _context.Javascript.FirstOrDefaultAsync(x => x.Name == name);
            if (data == null)
            {
                await _context.Javascript.AddAsync(new Javascript
                {
                    ID = Guid.NewGuid(),
                    Name = name,
                    Content = content
                });
            }
            else
            {
                data.Content = content;
                _context.Javascript.Update(data);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AppendJavascriptDataAsync(string name, string content)
        {
            Javascript? data = await _context.Javascript.FirstOrDefaultAsync(x => x.Name == name);
            if (data == null)
            {
                await _context.Javascript.AddAsync(new Javascript
                {
                    ID = Guid.NewGuid(),
                    Name = name,
                    Content = content
                });
            }
            else
            {
                data.Content += content;
                _context.Javascript.Update(data);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveJavascriptlAsync(List<string?> names)
        {
            foreach (string? name in names)
            {
                Javascript? data = await _context.Javascript.FirstOrDefaultAsync(x => x.Name == name);
                if (data == null) { }
                else
                {
                    _context.Javascript.Remove(data);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetAllPopupNamesAsync()
        {
            List<Popup> data = await _context.Popups.ToListAsync();
            List<string> names = new();
            foreach (Popup item in data)
            {
                if (item.Name == null) { }
                else
                {
                    names.Add(item.Name);
                }
            }

            return names;
        }

        public async Task<string?> GetPopupDataAsync(string name)
        {
            Popup? data = await _context.Popups.FirstOrDefaultAsync(p => p.Name == name);
            if (data == null)
            {
                return null;
            }

            Html? content = await _context.HTML.FirstOrDefaultAsync(c => c.ID == data.HtmlId);

            if (content == null) {
                return null;
            }
            return content.Content;

        }

        public async Task AddPopupDataAsync(string name, string content)
        {
            Popup? data = await _context.Popups.FirstOrDefaultAsync(p => p.Name == name);
            Html? dataContent = null;

            if (data == null)
            {
                dataContent = new Html
                {
                    ID = Guid.NewGuid(),
                    Name = name,
                    Content = content
                };

                await _context.HTML.AddAsync(dataContent);

                await _context.Popups.AddAsync(new Popup
                {
                    ID = Guid.NewGuid(),
                    Name = name,
                    HtmlId = dataContent.ID
                });
            }
            else
            {
                dataContent = await _context.HTML.FirstOrDefaultAsync(c => c.ID == data.HtmlId);

                if (dataContent == null)
                {
                    dataContent = new Html
                    {
                        ID = Guid.NewGuid(),
                        Name = name,
                        Content = content
                    };
                    await _context.HTML.AddAsync(dataContent);

                }
                else
                {
                    dataContent.Content = content;
                    _context.HTML.Update(dataContent);
                }

                data.HtmlId = dataContent.ID;
                _context.Popups.Update(data);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<GetPostMapData?> GetMapDataAsync()
        {
            MapInfo? mapInfo = await _context.MapInfo.FirstOrDefaultAsync();

            if (mapInfo == null)
            {
                return null;
            }

            GetPostMapData mapData = new GetPostMapData
            {
                BoundX = 0,
                BoundY = 0,
                DefaultX = 0,
                DefaultY = 0,
                HigherX = 0,
                HigherY = 0,
                ImageHeight = 0,
                ImageWidth = 0,
                LowerX = 0,
                LowerY = 0,
                PopupContentList = new List<PopupContent>(),
                SelectedCss = "",
                SelectedJs = ""
            };

            List<MapPopups> mapPopups = await _context.MapPopups.ToListAsync();


            foreach (MapPopups popup in mapPopups)
            {
                PopupLocation? popupLocation =  await _context.PopupLocation.FirstOrDefaultAsync(p => p.ID == popup.PopupLocationId);
                Popup? popupData = await _context.Popups.FirstOrDefaultAsync(p => p.ID == popup.PopupId);

                if( popupData == null || popupLocation == null ) 
                {
                    continue;
                }

                mapData.PopupContentList.Add(new PopupContent
                {
                    LocationX = popupLocation.LocationX,
                    LocationY = popupLocation.LocationY,
                    ID = Guid.NewGuid(),
                    ContentName = popupData.Name
                });
            }

            if (mapInfo?.CurrentCssId == null) {
                
            }
            else
            {
                Css? css = await _context.Css.FirstOrDefaultAsync(c => c.ID == mapInfo.CurrentCssId);

                if (css == null || css.Name == null)
                {

                }
                else
                {
                    mapData.SelectedCss = css.Name;
                }
            }

            if (mapInfo?.CurrentJsId == null) { }
            else
            {
                Javascript? js = await _context.Javascript.FirstOrDefaultAsync(j => j.ID == mapInfo.CurrentJsId);

                if (js == null || js.Name == null)
                {

                }
                else
                {
                    mapData.SelectedJs = js.Name;
                }
            }

            if(mapInfo?.DefaultX == null || mapInfo.DefaultY == null) { }
            else
            {
                mapData.DefaultX = (int)mapInfo.DefaultX;
                mapData.DefaultY = (int)mapInfo.DefaultY;
            }

            if (mapInfo?.HigherX == null || mapInfo.HigherY == null) { }
            else
            {
                mapData.HigherX = (int)mapInfo.HigherX;
                mapData.HigherY = (int)mapInfo.HigherY;
            }

            if (mapInfo?.LowerX == null || mapInfo.LowerY == null) { }
            else
            {
                mapData.LowerX = (int)mapInfo.LowerX;
                mapData.LowerY = (int)mapInfo.LowerY;
            }

            if (mapInfo?.BoundX == null || mapInfo.BoundY == null) { }
            else
            {
                mapData.BoundX = (int)mapInfo.BoundX;
                mapData.BoundY = (int)mapInfo.BoundY;
            }

            if (mapInfo?.ImageHeight == null || mapInfo.ImageWidth == null) { }
            else
            {
                mapData.ImageHeight = (int)mapInfo.ImageHeight;
                mapData.ImageWidth = (int)mapInfo.ImageWidth;
            }

            return mapData;

        }

        public async Task UpdateMapDataAsync(GetPostMapData mapData)
        {
            MapInfo? mapInfo = await _context.MapInfo.FirstOrDefaultAsync();
            bool newData = false;

            if(mapInfo == null)
            {
                newData = true;
                mapInfo = new MapInfo {
                    ID = Guid.NewGuid(),
                    CurrentCss = null,
                    CurrentJs = null,
                    CurrentCssId = null,
                    CurrentJsId = null,
                    DefaultX = null,
                    DefaultY = null,
                    HigherX = null,
                    HigherY = null,
                    LowerX = null,
                    LowerY = null,
                    BoundX = null, 
                    BoundY = null,
                    ImageHeight = null,
                    ImageWidth = null
                };
            }

            List<MapPopups> mapPopups = new List<MapPopups>();

            _context.MapPopups.RemoveRange(_context.MapPopups);
            await _context.SaveChangesAsync();

            mapInfo = await _context.MapInfo.FirstOrDefaultAsync();

            foreach (PopupContent popup in mapData.PopupContentList)
            {
                Popup? popupDatabase = await _context.Popups.FirstOrDefaultAsync(h => h.Name == popup.ContentName);

                if(popupDatabase == null || popup.LocationX == null || popup.LocationY == null)
                {
                    continue;
                }

                PopupLocation popupLocation = new PopupLocation
                {
                    ID = Guid.NewGuid(),
                    LocationX = (int)popup.LocationX,
                    LocationY = (int)popup.LocationY,
                };

                MapPopups newPopup = new MapPopups
                {
                    Popup = popupDatabase,
                    PopupLocation = popupLocation,
                    PopupId = popupDatabase.ID,
                    PopupLocationId = popupLocation.ID,
                };

                mapPopups.Add(newPopup);
            }

            await _context.MapPopups.AddRangeAsync(mapPopups);
            await _context.SaveChangesAsync();

            Css? css = await _context.Css.FirstOrDefaultAsync(c => c.Name == mapData.SelectedCss);

            if(css == null)
            {

            }
            else
            {
                mapInfo.CurrentCss = css;
                mapInfo.CurrentCssId = css.ID;
            }

            Javascript? js = await _context.Javascript.FirstOrDefaultAsync(j => j.Name == mapData.SelectedJs);

            if (js == null)
            {

            }
            else
            {
                mapInfo.CurrentJs = js;
                mapInfo.CurrentJsId = js.ID;
            }

            mapInfo.BoundX = mapData.BoundX;
            mapInfo.BoundY = mapData.BoundY;
            mapInfo.HigherX = mapData.HigherX;
            mapInfo.HigherY = mapData.HigherY;
            mapInfo.LowerY = mapData.LowerY;
            mapInfo.LowerX = mapData.LowerX;
            mapInfo.DefaultX = mapData.DefaultX;
            mapInfo.DefaultY = mapData.DefaultY;
            mapInfo.ImageHeight = mapData.ImageHeight;
            mapInfo.ImageWidth = mapData.ImageWidth;


            if (newData)
            {
                await _context.MapInfo.AddAsync(mapInfo);
            }
            else
            {
                _context.MapInfo.Update(mapInfo);
            }

            await _context.SaveChangesAsync();
        }
    }
}
