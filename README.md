# Assembly Service Registrar

**EN:** Assembly Service Registrar is a .NET library that provides automatic service registration for dependency injection containers using marker interfaces. This library simplifies the process of registering services by automatically scanning assemblies and registering implementations based on interface markers.

**TR:** Assembly Service Registrar, marker interface'ler kullanarak dependency injection container'ları için otomatik servis kaydı sağlayan bir .NET kütüphanesidir. Bu kütüphane, assembly'leri otomatik olarak tarayarak ve interface marker'larına göre implementation'ları kaydederek servis kayıt işlemini basitleştirir.

## 🚀 Features | Özellikler

**EN:**
- **Automatic Assembly Scanning**: Scans assemblies and automatically registers services
- **Marker Interface Pattern**: Uses marker interfaces to determine service lifetimes
- **Lifetime Management**: Supports Singleton, Scoped, and Transient lifetimes
- **Simple Integration**: Easy integration with Microsoft.Extensions.DependencyInjection
- **Convention-based Registration**: Automatically matches interfaces with implementations

**TR:**
- **Otomatik Assembly Tarama**: Assembly'leri tarar ve servisleri otomatik olarak kaydeder
- **Marker Interface Pattern**: Servis lifetime'larını belirlemek için marker interface'ler kullanır
- **Lifetime Yönetimi**: Singleton, Scoped ve Transient lifetime'larını destekler
- **Basit Entegrasyon**: Microsoft.Extensions.DependencyInjection ile kolay entegrasyon
- **Convention Tabanlı Kayıt**: Interface'leri implementation'larla otomatik eşleştirir

## 📋 Requirements | Gereksinimler

- .NET 8.0 or higher | .NET 8.0 veya üzeri
- Microsoft.Extensions.DependencyInjection package

## 🔧 Installation | Kurulum

### Package Installation | Paket Kurulumu

**EN:** Add the package to your project using one of the following methods:

**TR:** Aşağıdaki yöntemlerden birini kullanarak paketi projenize ekleyin:

#### NuGet Package Manager

```bash
dotnet add package AssemblyServiceRegistrar
```

#### Package Manager Console (Visual Studio)

```powershell
Install-Package AssemblyServiceRegistrar
```

#### PackageReference (in .csproj file)

```xml
<PackageReference Include="AssemblyServiceRegistrar" Version="1.0.0" />
```

### Git Clone

```bash
git clone https://github.com/sametbrr/assembly_service_registrar.git
cd assembly_service_registrar
dotnet build
```

## 🚀 Usage | Kullanım

### Basic Usage | Temel Kullanım

**EN:** First, create your service interfaces by inheriting from the appropriate marker interfaces:

**TR:** Öncelikle, uygun marker interface'lerden türeterek servis interface'lerinizi oluşturun:

```csharp
using AssemblyServiceRegistrar;

// For Singleton services | Singleton servisler için
public interface IConfigurationService : ISingletonService
{
    string GetConnectionString();
}

// For Scoped services | Scoped servisler için  
public interface IUserService : IScopedService
{
    Task<User> GetUserAsync(int id);
    Task CreateUserAsync(User user);
}

// For Transient services | Transient servisler için
public interface IEmailService : ITransientService
{
    Task SendEmailAsync(string to, string subject, string body);
}
```

### Service Implementations | Servis Implementation'ları

```csharp
// Singleton service implementation
public class ConfigurationService : IConfigurationService
{
    public string GetConnectionString()
    {
        return "Server=localhost;Database=MyApp;";
    }
}

// Scoped service implementation
public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    
    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<User> GetUserAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }
    
    public async Task CreateUserAsync(User user)
    {
        await _userRepository.AddAsync(user);
    }
}

// Transient service implementation
public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        // Email sending logic
        await Task.CompletedTask;
    }
}
```

### Complete Example | Tam Örnek

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AssemblyServiceRegistrar;
using System.Reflection;

// Program.cs
var builder = Host.CreateDefaultBuilder(args);

builder.services.AddServicesFromAssembly(Assembly.GetExecutingAssembly());

var host = builder.Build();

```

## 🔍 How It Works | Nasıl Çalışır

**EN:**
1. The library scans all types in the specified assembly
2. Finds all classes that implement interfaces derived from `IService`
3. Determines the service lifetime based on marker interfaces:
   - `ISingletonService` → `ServiceLifetime.Singleton`
   - `IScopedService` → `ServiceLifetime.Scoped`
   - `ITransientService` → `ServiceLifetime.Transient`
   - If no marker interface is found, uses the provided default lifetime
4. Registers the service with the DI container

**TR:**
1. Kütüphane belirtilen assembly'deki tüm tipleri tarar
2. `IService`'den türeyen interface'leri implement eden tüm class'ları bulur
3. Marker interface'lere göre servis lifetime'ını belirler:
   - `ISingletonService` → `ServiceLifetime.Singleton`
   - `IScopedService` → `ServiceLifetime.Scoped`
   - `ITransientService` → `ServiceLifetime.Transient`
   - Eğer marker interface bulunamazsa, sağlanan varsayılan lifetime'ı kullanır
4. Servisi DI container'a kaydeder

## 📚 Marker Interfaces | Marker Interface'ler

```csharp
namespace AssemblyServiceRegistrar
{
    // Base marker interface | Temel marker interface
    public interface IService { }
    
    // Scoped lifetime marker | Scoped lifetime marker'ı
    public interface IScopedService : IService { }
    
    // Singleton lifetime marker | Singleton lifetime marker'ı  
    public interface ISingletonService : IService { }
    
    // Transient lifetime marker | Transient lifetime marker'ı
    public interface ITransientService : IService { }
}
```
## 🎯 Best Practices | En İyi Uygulamalar

**EN:**
- Use `ISingletonService` for stateless services and configurations
- Use `IScopedService` for services that should be shared within a request scope
- Use `ITransientService` for lightweight, stateless services
- Keep your service interfaces focused and follow the Single Responsibility Principle

**TR:**
- Stateless servisler ve konfigürasyonlar için `ISingletonService` kullanın
- Request scope içinde paylaşılması gereken servisler için `IScopedService` kullanın
- Hafif, stateless servisler için `ITransientService` kullanın
- Servis interface'lerinizi odaklı tutun ve Single Responsibility Principle'ı takip edin

## ⚠️ Limitations | Sınırlamalar

**EN:**
- Only works with interfaces that inherit from `IService`
- Does not support generic service registration
- Requires marker interfaces for lifetime determination
- One interface per implementation (no multiple interface implementations)

**TR:**
- Sadece `IService`'den türeyen interface'lerle çalışır
- Generic servis kayıtlarını desteklemez
- Lifetime belirleme için marker interface'ler gerektirir
- Her implementation için bir interface (çoklu interface implementation'ları desteklemez)

## 📄 License | Lisans

This project is licensed under the MIT License

Bu proje MIT Lisansı altında lisanslanmıştır
