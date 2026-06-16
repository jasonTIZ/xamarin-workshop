# 🏥 Workshop Xamarin — Guía para la Clase
## Mini App de Tareas Médicas

> **Tiempo estimado:** 30 minutos  
> **Lo que vas a construir:** Una app Android que se conecta a un API real, carga una lista de tareas médicas y permite marcarlas como completadas.  
> **No necesitás experiencia previa con Xamarin.**

---

## ✅ Antes de empezar — Requisitos

### Para Windows

Necesitás tener instalado en tu computadora:

| Herramienta | Versión mínima |
|---|---|
| Visual Studio 2022 Community | 17.x |
| Workload "Desarrollo móvil con .NET" | (se instala dentro de VS) |
| Android Emulator | (se instala dentro de VS) |

### Para Linux (Debian/Ubuntu/Arch y derivados)

- **.NET 9.0 SDK** (para ejecutar el API)
- **Docker** (para compilar la app Xamarin)
- **Android SDK Platform Tools** (`adb`, para instalar en dispositivo físico)
- **Un celular Android** con modo desarrollador y depuración USB activados
- **Bash, Zsh o Fish** (cualquier shell moderno — los scripts usan `bash` internamente)

> **Nota importante:** En Linux no es posible compilar apps Xamarin de forma local (no hay soporte oficial). Por eso usamos Docker con una imagen preconfigurada con todos los componentes de Xamarin Android.

---

Si no tenés Visual Studio instalado (Windows) o los requisitos de Linux, seguí los pasos de instalación a continuación. Si ya los tenés, saltá directo a la [Sección 2](#2-clonar-el-proyecto).

---

## 1. Instalación de herramientas

### 1.1 Windows — Instalación de Visual Studio 2022

#### 1.1.1 Descargar el instalador

Entrá a:
```
https://visualstudio.microsoft.com/vs/community/
```
Descargá **Visual Studio Community 2022** (es gratuito).

#### 1.1.2 Ejecutar el instalador

Al abrir el instalador, te va a pedir que elijas los **workloads**. Buscá y marcá el siguiente:

```
✅ Desarrollo móvil con .NET
```

> ⚠️ Este workload descarga el Android SDK, el emulador y todo lo necesario. Ocupa aproximadamente **8-15 GB**. Asegurate de tener espacio en disco y buena conexión.

Dejá todo lo demás por defecto y presioná **Instalar**.

#### 1.1.3 Crear un emulador Android

Una vez instalado Visual Studio:

1. Abrí Visual Studio
2. Ir a **Herramientas → Android → Administrador de dispositivos Android**
3. Clic en **+ Nuevo**
4. Configuración sugerida:
   - **Dispositivo base:** Pixel 5
   - **SO:** Android 13.0 (API 33)
   - **Arquitectura:** x86_64
5. Clic en **Crear**
6. Una vez creado, presioná ▶ para iniciarlo y verificar que arranca

> ⚠️ La primera vez que iniciás el emulador puede tardar 3-5 minutos. Inicialo mientras creás el proyecto.

---

### 1.2 Linux — Instalación de requisitos

#### 1.2.1 Instalar .NET 9.0 SDK

**Debian/Ubuntu:**
```bash
sudo apt update
sudo apt install -y dotnet-sdk-9.0
```

**Arch Linux:**
```bash
sudo pacman -S dotnet-sdk
```

Verificá la instalación:
```bash
dotnet --version
# Debería mostrar: 9.0.x
```

#### 1.2.2 Instalar Docker

**Debian/Ubuntu:**
```bash
sudo apt update
sudo apt install -y docker.io
sudo systemctl enable --now docker
sudo usermod -aG docker $USER
```

**Arch Linux:**
```bash
sudo pacman -S docker
sudo systemctl enable --now docker
sudo usermod -aG docker $USER
```

> **Cerrá sesión y volvé a entrar** para que el grupo `docker` surta efecto.  
> Alternativamente, ejecutá `newgrp docker` en la terminal actual para activarlo sin cerrar sesión.  
> **Usuarios de Fish:** `newgrp docker` te va a dejar en una subshell Bash — es normal. Si preferís, cerrá y abrí sesión directamente.

Verificá la instalación:
```bash
docker --version
docker run --rm hello-world
```

#### 1.2.3 Instalar Android SDK Platform Tools (adb)

> **Si ya tenés Android Studio instalado (es obligatorio para el curso), `adb` ya viene incluido — no necesitás instalar nada extra.** Solo asegurate de que esté en tu PATH:
> ```bash
> # La ruta habitual de Android Studio
> export PATH="$PATH:$HOME/Android/Sdk/platform-tools"
> ```
> Agregá esa línea al final de tu `~/.bashrc`, `~/.zshrc`, o `~/.config/fish/config.fish` (en Fish usá `fish_add_path $HOME/Android/Sdk/platform-tools`), y luego abrí una terminal nueva.

Si **no** tenés Android Studio, instalá las platform-tools por separado:

**Debian/Ubuntu:**
```bash
sudo apt update
sudo apt install -y android-sdk-platform-tools
```

**Arch Linux:**
```bash
sudo pacman -S android-sdk-platform-tools
```

Verificá la instalación:
```bash
adb version
```

Conectá tu celular por USB y asegurate de tener **modo desarrollador** y **depuración USB** activados. Verificá que lo detecta:
```bash
adb devices
# Debería mostrar algo como: ABC123XYZ    device
```

Si aparece `unauthorized`, desbloqueá el celular y aceptá el diálogo "Permitir depuración USB".

---

## 2. Clonar el proyecto

El repositorio ya incluye la solución Xamarin completa, así que **nadie necesita crear un proyecto nuevo** — solo clonarlo.

### 2.1 Windows — Clonar y abrir en Visual Studio

Cloná el repositorio. Podés hacerlo desde la terminal de git o desde Visual Studio:

**Opción A — Git Bash / PowerShell / CMD:**
```
git clone git@github.com:jasonTIZ/xamarin-workshop.git
```

**Opción B — Desde Visual Studio:**
1. En la pantalla de inicio, clic en **Clonar un repositorio**
2. Pegá la URL: `git@github.com:jasonTIZ/xamarin-workshop.git`
3. Elegí la carpeta de destino y clic en **Clonar**

Una vez clonado:
1. Abrir Visual Studio 2022
2. **Archivo → Abrir → Proyecto o solución**
3. Navegar a `xamarin-workshop/MedicalTasks/MedicalTasks.sln`
4. Esperar a que Visual Studio restaure los paquetes NuGet automáticamente

### 2.2 Linux — Clonar y preparar el proyecto

```bash
git clone git@github.com:jasonTIZ/xamarin-workshop.git
cd xamarin-workshop
```

Dále permisos de ejecución a los scripts:
```bash
chmod +x run-api.sh build-android.sh
```

Iniciá el API para verificar que todo funciona:
```bash
./run-api.sh
# Debería mostrar: Now listening on: http://0.0.0.0:5000
```

> **Dejá esta terminal abierta** — el API corre en primer plano. Abrí una terminal nueva para los pasos siguientes.

---

## 3. Estructura de carpetas

### 3.1 Windows — Visual Studio

En el **Explorador de soluciones** (panel derecho), localizar el proyecto `MedicalTasks` (el principal, no el `.Android`).

Crear las siguientes 4 carpetas haciendo **clic derecho sobre `MedicalTasks` → Agregar → Nueva carpeta**:

```
MedicalTasks/
├── Models/        ← datos
├── ViewModels/    ← lógica
├── Views/         ← pantallas
└── Converters/    ← transformadores visuales
```

### 3.2 Linux — Terminal

Desde la raíz del proyecto:

```bash
mkdir -p MedicalTasks/MedicalTasks/{Models,ViewModels,Views,Converters}
```

> Esto funciona igual en Bash, Zsh y Fish.

---

## 4. Crear el Model

El Model representa los datos que nos devuelve el API.

**Windows:** Clic derecho sobre la carpeta `Models` → Agregar → Clase → Nombre: `MedicalTask.cs`

**Linux:**
```bash
touch MedicalTasks/MedicalTasks/Models/MedicalTask.cs
```

Reemplazar todo el contenido del archivo con:

```csharp
namespace MedicalTasks.Models
{
    public class MedicalTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}
```

Guardar el archivo (`Ctrl + S` en Windows y en la mayoría de editores de Linux).

---

## 5. Crear el ViewModel

El ViewModel contiene toda la lógica: cargar datos del API, manejar errores y ejecutar acciones.

**Windows:** Clic derecho sobre la carpeta `ViewModels` → Agregar → Clase → Nombre: `TaskListViewModel.cs`

**Linux:**
```bash
touch MedicalTasks/MedicalTasks/ViewModels/TaskListViewModel.cs
```

> **Atención — la URL del API es diferente según tu sistema:**
> - **Windows (emulador):** `http://10.0.2.2:5000/api` — el emulador usa esta IP para ver el `localhost` de tu PC
> - **Linux (dispositivo físico):** `http://127.0.0.1:5000/api` — el script `build-android.sh` configura un túnel USB (`adb reverse`) para que el celular acceda al API de tu PC por esa dirección
>
> En el código de abajo está puesta la versión de Windows. **Si estás en Linux, cambiá esa línea** tal como se indica en el comentario.

Reemplazar todo el contenido con:

```csharp
using MedicalTasks.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MedicalTasks.ViewModels
{
    public class TaskListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // ── Propiedades que la View va a observar ──────────────────

        private ObservableCollection<MedicalTask> _tasks;
        public ObservableCollection<MedicalTask> Tasks
        {
            get => _tasks;
            set { _tasks = value; OnPropertyChanged(); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasError)); }
        }

        public bool HasError => !string.IsNullOrEmpty(_errorMessage);

        // ── Comandos ───────────────────────────────────────────────

        public ICommand LoadTasksCommand { get; }
        public ICommand CompleteTaskCommand { get; }

        // ── URL del API ────────────────────────────────────────────
        // Windows (emulador):         "http://10.0.2.2:5000/api"
        // Linux  (dispositivo físico): "http://127.0.0.1:5000/api"  ← cambiá esta línea si estás en Linux
        private const string ApiBaseUrl = "http://10.0.2.2:5000/api";

        // ── Constructor ───────────────────────────────────────────

        public TaskListViewModel()
        {
            LoadTasksCommand = new Command(async () => await LoadTasksAsync());
            CompleteTaskCommand = new Command<MedicalTask>(async (task) => await CompleteTaskAsync(task));
        }

        // ── Cargar tareas desde el API ─────────────────────────────

        private async Task LoadTasksAsync()
        {
            IsLoading = true;
            ErrorMessage = null;

            try
            {
                var client = new HttpClient();
                var json = await client.GetStringAsync($"{ApiBaseUrl}/tasks");

                Tasks = JsonConvert.DeserializeObject<ObservableCollection<MedicalTask>>(json);
            }
            catch (HttpRequestException)
            {
                ErrorMessage = "No se pudo conectar al servidor.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── Marcar una tarea como completada ──────────────────────

        private async Task CompleteTaskAsync(MedicalTask task)
        {
            if (task == null || task.IsCompleted) return;

            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{ApiBaseUrl}/tasks/{task.Id}/complete")
                {
                    Content = new StringContent("")
                };
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                    await LoadTasksAsync();
            }
            catch (HttpRequestException)
            {
                ErrorMessage = "Error al actualizar la tarea.";
            }
        }

        // ── Notificación de cambios a la View ─────────────────────

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

Guardar el archivo.

---

## 6. Crear los Converters

Los converters traducen datos del ViewModel a valores visuales (colores, visibilidad).

### 6.1 BoolToColorConverter

**Windows:** Clic derecho sobre la carpeta `Converters` → Agregar → Clase → Nombre: `BoolToColorConverter.cs`

**Linux:**
```bash
touch MedicalTasks/MedicalTasks/Converters/BoolToColorConverter.cs
```

```csharp
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MedicalTasks.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCompleted)
                return isCompleted ? Color.FromHex("#D4EDDA") : Color.White;

            return Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
```

### 6.2 InverseBoolConverter

**Windows:** Clic derecho sobre la carpeta `Converters` → Agregar → Clase → Nombre: `InverseBoolConverter.cs`

**Linux:**
```bash
touch MedicalTasks/MedicalTasks/Converters/InverseBoolConverter.cs
```

```csharp
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MedicalTasks.Converters
{
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b) return !b;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b) return !b;
            return false;
        }
    }
}
```

Guardar ambos archivos.

---

## 7. Registrar los Converters en App.xaml

Abrir el archivo `App.xaml`:
- **Windows:** doble clic desde el Explorador de Soluciones (raíz de `MedicalTasks`)
- **Linux:** `MedicalTasks/MedicalTasks/App.xaml`

Reemplazar **todo** su contenido con:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Application xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:converters="clr-namespace:MedicalTasks.Converters"
             x:Class="MedicalTasks.App">

    <Application.Resources>
        <ResourceDictionary>
            <converters:BoolToColorConverter x:Key="BoolToColorConverter" />
            <converters:InverseBoolConverter x:Key="InverseBoolConverter" />
        </ResourceDictionary>
    </Application.Resources>

</Application>
```

Guardar el archivo.

---

## 8. Crear la pantalla principal (View)

### 8.1 Agregar la página

**Windows:**
1. Clic derecho sobre la carpeta `Views` → Agregar → Nuevo elemento
2. En el buscador escribir `contenido` y seleccionar **"Página de contenido"**
3. Nombre: `TaskListPage.xaml` → Agregar

> ⚠️ Asegurate de elegir "Página de contenido", no "Clase". Debe generar dos archivos: `TaskListPage.xaml` y `TaskListPage.xaml.cs`.

**Linux:**
```bash
touch MedicalTasks/MedicalTasks/Views/TaskListPage.xaml
touch MedicalTasks/MedicalTasks/Views/TaskListPage.xaml.cs
```

### 8.2 Diseñar la pantalla en XAML

Abrir `TaskListPage.xaml` (Linux: `MedicalTasks/MedicalTasks/Views/TaskListPage.xaml`) y reemplazar **todo** su contenido con:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MedicalTasks.Views.TaskListPage"
             Title="Tareas Médicas">

    <ContentPage.Content>
        <StackLayout Padding="16" BackgroundColor="#F5F5F5">

            <!-- Título -->
            <Label Text="📋 Tareas del Día"
                   FontSize="22"
                   FontAttributes="Bold"
                   TextColor="#1F4E79"
                   Margin="0,0,0,12"/>

            <!-- Spinner de carga -->
            <ActivityIndicator IsRunning="{Binding IsLoading}"
                               IsVisible="{Binding IsLoading}"
                               Color="#2E75B6"
                               HeightRequest="40"/>

            <!-- Mensaje de error -->
            <Label Text="{Binding ErrorMessage}"
                   IsVisible="{Binding HasError}"
                   TextColor="Red"
                   FontSize="14"
                   Margin="0,0,0,8"/>

            <!-- Lista de tareas -->
            <CollectionView ItemsSource="{Binding Tasks}"
                            SelectionMode="None">

                <!-- Vista cuando la lista está vacía -->
                <CollectionView.EmptyView>
                    <StackLayout HorizontalOptions="Center"
                                 Margin="0,40,0,0">
                        <Label Text="✅"
                               FontSize="48"
                               HorizontalOptions="Center"/>
                        <Label Text="No hay tareas pendientes"
                               FontSize="16"
                               TextColor="#666666"
                               HorizontalOptions="Center"/>
                    </StackLayout>
                </CollectionView.EmptyView>

                <!-- Plantilla de cada tarjeta -->
                <CollectionView.ItemTemplate>
                    <DataTemplate>
                        <Frame Margin="0,4"
                               Padding="14"
                               CornerRadius="8"
                               HasShadow="True"
                               BackgroundColor="{Binding IsCompleted,
                                   Converter={StaticResource BoolToColorConverter}}">

                            <Grid ColumnDefinitions="*,Auto"
                                  RowDefinitions="Auto,Auto">

                                <!-- Nombre de la tarea -->
                                <Label Grid.Column="0" Grid.Row="0"
                                       Text="{Binding Title}"
                                       FontSize="15"
                                       TextColor="#333333"
                                       VerticalOptions="Center"/>

                                <!-- Estado: Pendiente / Completada -->
                                <Label Grid.Column="0" Grid.Row="1"
                                       FontSize="12"
                                       TextColor="#888888"
                                       Margin="0,4,0,0">
                                    <Label.Triggers>
                                        <DataTrigger TargetType="Label"
                                                     Binding="{Binding IsCompleted}"
                                                     Value="True">
                                            <Setter Property="Text" Value="✅ Completada"/>
                                        </DataTrigger>
                                        <DataTrigger TargetType="Label"
                                                     Binding="{Binding IsCompleted}"
                                                     Value="False">
                                            <Setter Property="Text" Value="⏳ Pendiente"/>
                                        </DataTrigger>
                                    </Label.Triggers>
                                </Label>

                                <!-- Botón completar (solo en tareas pendientes) -->
                                <Button Grid.Column="1" Grid.RowSpan="2"
                                        Text="✓"
                                        FontSize="18"
                                        IsVisible="{Binding IsCompleted,
                                            Converter={StaticResource InverseBoolConverter}}"
                                        BackgroundColor="#2E75B6"
                                        TextColor="White"
                                        CornerRadius="20"
                                        WidthRequest="44"
                                        HeightRequest="44"
                                        Padding="0"
                                        VerticalOptions="Center"
                                        Command="{Binding Source={RelativeSource AncestorType={x:Type ContentPage}},
                                                  Path=BindingContext.CompleteTaskCommand}"
                                        CommandParameter="{Binding .}"/>
                            </Grid>
                        </Frame>
                    </DataTemplate>
                </CollectionView.ItemTemplate>
            </CollectionView>

            <!-- Botón recargar -->
            <Button Text="↻  Recargar tareas"
                    Command="{Binding LoadTasksCommand}"
                    BackgroundColor="#1F4E79"
                    TextColor="White"
                    CornerRadius="8"
                    Margin="0,16,0,0"
                    HeightRequest="48"/>

        </StackLayout>
    </ContentPage.Content>

</ContentPage>
```

Guardar el archivo.

---

## 9. Conectar el ViewModel a la pantalla

Abrir `Views/TaskListPage.xaml.cs` (Linux: `MedicalTasks/MedicalTasks/Views/TaskListPage.xaml.cs`) y reemplazar **todo** su contenido con:

```csharp
using MedicalTasks.ViewModels;
using Xamarin.Forms;

namespace MedicalTasks.Views
{
    public partial class TaskListPage : ContentPage
    {
        public TaskListPage()
        {
            InitializeComponent();
            BindingContext = new TaskListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = (TaskListViewModel)BindingContext;
            vm.LoadTasksCommand.Execute(null);
        }
    }
}
```

Guardar el archivo.

---

## 10. Configurar la pantalla de inicio

Abrir `App.xaml.cs` (Linux: `MedicalTasks/MedicalTasks/App.xaml.cs`) y reemplazar **todo** su contenido con:

```csharp
using MedicalTasks.Views;
using Xamarin.Forms;

namespace MedicalTasks
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new TaskListPage())
            {
                BarBackgroundColor = Color.FromHex("#1F4E79"),
                BarTextColor = Color.White
            };
        }

        protected override void OnStart() { }
        protected override void OnSleep() { }
        protected override void OnResume() { }
    }
}
```

Guardar el archivo.

---

## 11. Configurar permisos de Android

### 11.1 Permiso de internet

Abrir `AndroidManifest.xml`:
- **Windows:** `MedicalTasks.Android/Properties/AndroidManifest.xml` desde el Explorador de Soluciones
- **Linux:** `MedicalTasks/MedicalTasks.Android/Properties/AndroidManifest.xml`

Verificar que dentro del tag `<manifest>` existe esta línea:
```xml
<uses-permission android:name="android.permission.INTERNET" />
```

Si no está, agregarla.

### 11.2 Permitir tráfico HTTP

**Windows:** En `MedicalTasks.Android/Resources`, crear la carpeta `xml` (clic derecho → Agregar → Nueva carpeta) y dentro crear `network_security_config.xml` (clic derecho sobre `xml` → Agregar → Nuevo elemento → Archivo XML).

**Linux:**
```bash
mkdir -p MedicalTasks/MedicalTasks.Android/Resources/xml
touch MedicalTasks/MedicalTasks.Android/Resources/xml/network_security_config.xml
```

Contenido del archivo (el mismo para Windows y Linux — incluye las IPs de ambos):
```xml
<?xml version="1.0" encoding="utf-8"?>
<network-security-config>
    <domain-config cleartextTrafficPermitted="true">
        <domain includeSubdomains="true">10.0.2.2</domain>
        <domain includeSubdomains="true">127.0.0.1</domain>
    </domain-config>
</network-security-config>
```

> Incluimos ambas IPs para que el mismo XML funcione en Windows (emulador usa `10.0.2.2`) y Linux (dispositivo físico usa `127.0.0.1` via `adb reverse`).

**Solo Linux — registrar el archivo en el proyecto:**

En proyectos Xamarin de estilo antiguo, los archivos creados desde la terminal no se agregan solos al `.csproj`. Visual Studio en Windows lo hace automáticamente, pero en Linux hay que hacerlo a mano. Abrir `MedicalTasks/MedicalTasks.Android/MedicalTasks.Android.csproj` y agregar esta línea dentro del `<ItemGroup>` que ya tiene otros `<AndroidResource>`:

```xml
<AndroidResource Include="Resources\xml\network_security_config.xml" />
```

Debe quedar junto a las otras entradas de recursos, por ejemplo:
```xml
<AndroidResource Include="Resources\drawable\icon_feed.png" />
<AndroidResource Include="Resources\xml\network_security_config.xml" />  <!-- ← agregar -->
```

Luego abrir nuevamente `AndroidManifest.xml` y dentro del tag `<application>` agregar:
```xml
android:networkSecurityConfig="@xml/network_security_config"
```

Debe quedar así:
```xml
<application android:label="MedicalTasks"
             android:theme="@style/MainTheme"
             android:networkSecurityConfig="@xml/network_security_config">
```

Guardar el archivo.

---

## 12. Ejecutar la app

### 12.1 Windows — Visual Studio

En la barra superior de Visual Studio, en el selector de dispositivos (donde dice "Android Emulators"), seleccionar el emulador que creaste en el paso 1.1.3.

> Si el emulador no aparece, abrirlo manualmente desde **Herramientas → Android → Administrador de dispositivos Android** y presionar ▶.

Presionar **F5** o el botón ▶ verde en la barra superior.

La primera compilación toma **1-2 minutos**. Las siguientes son más rápidas.

---

### 12.2 Linux — Docker + dispositivo físico

#### Paso 1 — Verificar que el celular está conectado

```bash
adb devices
# Debe mostrar algo como:
# List of devices attached
# ABC123XYZ    device
```

Si aparece `unauthorized`, desbloqueá el celular y aceptá el diálogo "Permitir depuración USB".

#### Paso 2 — Iniciar el API (si no lo hiciste antes)

Abrí una terminal y dejala corriendo:

```bash
./run-api.sh
# Debe mostrar: Now listening on: http://0.0.0.0:5000
```

#### Paso 3 — Compilar e instalar la app

En otra terminal, desde la raíz del proyecto:

```bash
./build-android.sh
```

El script hace todo automáticamente:
1. Construye la imagen Docker si no existe todavía
2. Compila la app Xamarin dentro del contenedor
3. Firma el APK
4. Ejecuta `adb reverse tcp:5000 tcp:5000` para que el celular pueda alcanzar el API de tu PC
5. Instala el APK en el celular

> **Primera vez:** La imagen Docker pesa ~2 GB y la compilación inicial puede tomar **5-10 minutos**. Las compilaciones siguientes son más rápidas gracias al caché de Docker.

> **El script funciona tal cual en Bash, Zsh y Fish** — tiene `#!/bin/bash` al inicio, así que usa Bash internamente sin importar qué shell estés usando.

---

### 12.3 Resultado esperado

Al abrir la app deberías ver:

- 🔵 Un spinner de carga brevemente mientras el API responde
- 📋 La lista de tareas médicas cargada desde el servidor
- Tareas pendientes con fondo **blanco** y botón **✓** azul
- Tareas completadas con fondo **verde** sin botón

Al presionar **✓** en una tarea:
- La tarea cambia a completada (fondo verde, botón desaparece)
- La lista se recarga automáticamente

---

## 🔍 ¿Qué acabas de construir?

| Concepto | Dónde lo aplicaste |
|---|---|
| **MVVM** | `TaskListViewModel` separado de `TaskListPage` |
| **Data Binding** | `{Binding IsLoading}`, `{Binding Tasks}`, etc. |
| **INotifyPropertyChanged** | Setters del ViewModel con `OnPropertyChanged()` |
| **ObservableCollection** | Lista de tareas que notifica cambios a la UI |
| **ICommand** | `LoadTasksCommand` y `CompleteTaskCommand` |
| **Converters** | Color de tarjeta y visibilidad del botón según estado |
| **async/await** | Llamadas HTTP sin bloquear la interfaz |
| **CollectionView** | Lista optimizada con EmptyView y DataTemplate |

---

## ❓ Preguntas frecuentes

**¿Por qué `10.0.2.2` en Windows y `127.0.0.1` en Linux?**  
En Windows se usa el emulador Android. Desde adentro del emulador, `localhost` apunta al propio emulador (no a tu PC), así que Android reserva la IP `10.0.2.2` específicamente para referirse al `localhost` de la máquina host. En Linux usamos un celular físico conectado por USB: el script corre `adb reverse tcp:5000 tcp:5000`, que crea un túnel por el cable USB para que el celular pueda acceder al puerto 5000 de tu PC usando `127.0.0.1`.

**¿Por qué necesito el `network_security_config.xml`?**  
Desde Android 9, las apps bloquean tráfico HTTP sin cifrar por defecto. Como el API usa HTTP en el entorno de desarrollo (no HTTPS), necesitamos autorizar explícitamente esas direcciones IP.

**¿Por qué `ObservableCollection` y no `List`?**  
`List` es una colección estática. `ObservableCollection` implementa `INotifyCollectionChanged`, lo que significa que avisa a la `CollectionView` cada vez que se agrega, elimina o modifica un elemento. Sin esto, la pantalla no se actualizaría al cambiar los datos.

**¿Qué hace `OnPropertyChanged()`?**  
Dispara el evento `PropertyChanged` que la View está escuchando. Cuando Xamarin.Forms ve ese evento, sabe que debe releer la propiedad y actualizar el control visual correspondiente.

**¿Puedo usar VS Code en Linux para editar los archivos?**  
Sí. Desde la raíz del proyecto ejecutá `code .` para abrirlo. Los archivos `.cs` y `.xaml` se editan normalmente. La compilación y el deploy se hacen siempre con `./build-android.sh`.

**VS Code me muestra errores rojos como "Predefined type 'System.Void' is not defined" en los archivos `.cs`. ¿Qué hago?**  
Ignoralos — son falsos positivos. El analizador de C# de VS Code (OmniSharp) no entiende proyectos Xamarin.Forms con `netstandard2.0` y marca todo como error aunque el código esté bien. La compilación real la hace MSBuild dentro del contenedor Docker y no tiene este problema. Si ves estos errores en VS Code, simplemente continuá escribiendo el código y compilá con `./build-android.sh` para verificar que todo está correcto.

**¿Qué pasa si Docker falla por falta de RAM?**  
La compilación Xamarin puede requerir hasta 3-4 GB de RAM. Si falla con un error de memoria, cerrá otras aplicaciones y volvé a intentar.

**¿Cómo sé qué IP tiene mi PC si quiero conectarme por WiFi en lugar de USB?**  
No es necesario para este workshop — el método con USB y `adb reverse` es más confiable. Pero si lo necesitás:
```bash
# Bash/Zsh/Fish
ip addr show | grep "inet " | grep -v 127.0.0.1
# Busca algo como 192.168.X.X
```

---

*Workshop — Diseño y Programación de Plataformas Móviles, I Ciclo 2026.*  
*Universidad Nacional, Sección Regional Huetar Norte y Caribe.*
