global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;    
global using System.Text;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.IdentityModel.Tokens;
global using System.ComponentModel.DataAnnotations;
global using API.Data;
global using API.Models;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.OpenApi.Models;
global using Microsoft.AspNetCore.SignalR;
global using API.Services.Token;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using API.AutoMapper;
global using Twilio;
global using Twilio.Rest.Api.V2010.Account;
global using Twilio.Types;
global using API.Services.Twilio;
global using API.Services.PDF;
global using API.Services.Email;
global using System;
global using System.IO;
global using System.Threading.Tasks;
global using iText.Html2pdf;
global using iText.Kernel.Pdf;
global using Microsoft.Extensions.Options;
global using Microsoft.AspNetCore.Mvc;
global using API.DTOs;
global using API.Services.Auth;
global using System.Text.RegularExpressions;
global using Tesseract;
global using API.Services.Document;
global using API.Controllers;
global using API.Services.Score;
global using API.Services.Student;
global using AutoMapper;
global using Microsoft.Extensions.Configuration;
global using API.Services.Api;
global using Newtonsoft.Json;