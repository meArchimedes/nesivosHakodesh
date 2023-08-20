const { app, BrowserWindow, Menu, screen, shell   } = require('electron')
const url = require("url");
const path = require("path");

const isMac = process.platform === 'darwin';
let appWindow

const template = [
    // { role: 'appMenu' }
    ...(isMac ? [{
      label: app.name,
      submenu: [
        { role: 'about' },
        { type: 'separator' },
        { role: 'services' },
        { type: 'separator' },
        { role: 'hide' },
        { role: 'hideothers' },
        { role: 'unhide' },
        { type: 'separator' },
        { role: 'quit' }
      ]
    }] : []),
    // { role: 'fileMenu' }
    {
      label: 'File',
      submenu: [
        isMac ? { role: 'close' } : { role: 'quit' }
      ]
    },
    // { role: 'editMenu' }
    {
      label: 'Edit',
      submenu: [
        { role: 'undo' },
        { role: 'redo' },
        { type: 'separator' },
        { role: 'cut' },
        { role: 'copy' },
        { role: 'paste' },
        ...(isMac ? [
          { role: 'pasteAndMatchStyle' },
          { role: 'delete' },
          { role: 'selectAll' },
          { type: 'separator' },
          {
            label: 'Speech',
            submenu: [
              { role: 'startSpeaking' },
              { role: 'stopSpeaking' }
            ]
          }
        ] : [
          { role: 'delete' },
          { type: 'separator' },
          { role: 'selectAll' }
        ])
      ]
    },
    // { role: 'viewMenu' }
    {
      label: 'View',
      submenu: [
        { role: 'reload' },
        //{ role: 'forceReload' },
        { role: 'toggleDevTools' },
        { type: 'separator' },
        { role: 'resetZoom' },
        { role: 'zoomIn' },
        { role: 'zoomOut' },
        { type: 'separator' },
        { role: 'togglefullscreen' }
      ]
    },
    // { role: 'windowMenu' }
    {
      label: 'Window',
      submenu: [
        { role: 'minimize' },
        { role: 'zoom' },
        ...(isMac ? [
          { type: 'separator' },
          { role: 'front' },
          { type: 'separator' },
          { role: 'window' }
        ] : [
          { role: 'close' }
        ])
      ]
    },
    /*{
      role: 'help',
      submenu: [
        {
          label: 'Learn More',
          click: async () => {
            const { shell } = require('electron')
            await shell.openExternal('https://mavensoftwaresolutions.com')
          }
        }
      ]
    }*/
  ]

function initWindow() {

    const electronScreen = screen;
    const size = electronScreen.getPrimaryDisplay().workAreaSize;

  appWindow = new BrowserWindow({
      x: 0,
      y: 0,
      //width: 1000,
      //height: 800,
      width: size.width,
      height: size.height,
      webPreferences: {
        nativeWindowOpen: true,
        nodeIntegration: true,
        contextIsolation: false,
        enableRemoteModule : true,
      }
  })

  // Electron Build Path
  appWindow.loadURL(
      url.format({
      pathname: path.join(__dirname, `/dist/index.html`),
      protocol: "file:",
      slashes: true
      })
  );

  //appWindow.webContents.openDevTools();

  const menu = Menu.buildFromTemplate(template)
  Menu.setApplicationMenu(menu)

  appWindow.on('closed', function () {
      appWindow = null
  })

  appWindow.webContents.setWindowOpenHandler(({ url }) => {
    
    var newUrl = url.replace("dist/#/", `dist/index.html/#/`);
    
    let win = new BrowserWindow({ 
      webPreferences: {
        webSecurity : false
      },
      show: false 
    })
    win.on('close', function () { win = null })
    win.loadURL(newUrl)
    win.once('ready-to-show', () => {
        win.show()
    })
  });
}

app.on('ready', initWindow)

// Close when all windows are closed.
app.on('window-all-closed', function () {

  // On macOS specific close process
  if (process.platform !== 'darwin') {
      app.quit()
  }
})

app.on('activate', function () {
  if (win === null) {
      initWindow()
  }
})
