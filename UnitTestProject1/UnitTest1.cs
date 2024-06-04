using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using System.Security.Cryptography;
using SeleniumExtras.WaitHelpers;
using System.Diagnostics;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private IWebDriver driver;
        [TestInitialize]
        public void Setup()
        {
            // Initializare Driver
            driver = new ChromeDriver();
        }
        [TestMethod]
        [TestCategory("Login")]
        public void AutentificareSiteCredentialeValide()
        {

            driver.Navigate().GoToUrl("https://www.saucedemo.com");
            driver.Manage().Window.Maximize();


            // Găsește elementele de autentificare
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Name("password"));


            // Completează câmpurile de autentificare
            usernameField.SendKeys("standard_user");
            System.Threading.Thread.Sleep(1000);
            passwordField.SendKeys("secret_sauce");
            System.Threading.Thread.Sleep(1000);

            IWebElement loginButton = driver.FindElement(By.Id("login-button"));

            // Apasă butonul de continua pentru autentificare
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);


            // Așteaptă pentru a se încărca pagina după autentificare
            System.Threading.Thread.Sleep(5000);

            // Verifică dacă autentificarea a reușit
            IWebElement productsTitle = driver.FindElement(By.ClassName("title"));
            Assert.IsTrue(productsTitle.Displayed, "Autentificarea cu credențiale valide a eșuat!");
        }
        [TestMethod]
        [TestCategory("Login")]
        public void AutentificareSiteCredentialeInvalide()
        {
            // Deschide pagina de autentificare
            driver.Navigate().GoToUrl("https://www.saucedemo.com");
            driver.Manage().Window.Maximize();

            // Introduce credențiale invalide
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            usernameField.SendKeys("utilizator_inexistent");
            System.Threading.Thread.Sleep(1000);

            IWebElement passwordField = driver.FindElement(By.Id("password"));
            passwordField.SendKeys("parola_gresita");
            System.Threading.Thread.Sleep(1000);

            IWebElement loginButton = driver.FindElement(By.Id("login-button"));
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);

            // Verifică dacă mesajul de eroare este afișat
            IWebElement errorElement = driver.FindElement(By.CssSelector("[data-test='error']"));
            Assert.IsTrue(errorElement.Displayed, "Autentificarea cu credențiale invalide nu a generat eroare!");

        }
        [TestMethod]
        [TestCategory("Sortare")]
        public void TestSortareProduseDupaPretCrescator()
        {
            // Deschide pagina de autentificare
            driver.Navigate().GoToUrl("https://www.saucedemo.com");
            driver.Manage().Window.Maximize();

            // Autentifică-te cu credențiale valide (adaptă acești pași în funcție de necesități)
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));

            usernameField.SendKeys("standard_user");
            System.Threading.Thread.Sleep(1000);
            passwordField.SendKeys("secret_sauce");
            System.Threading.Thread.Sleep(1000);
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);

            // Selectează opțiunea de sortare după preț
            IWebElement sortDropdown = driver.FindElement(By.ClassName("product_sort_container"));
            sortDropdown.Click();
            System.Threading.Thread.Sleep(1000);

            SelectElement selectSort = new SelectElement(sortDropdown);
            selectSort.SelectByText("Price (low to high)");
            System.Threading.Thread.Sleep(1000);

            // Așteaptă ca produsele să se sorteze
            System.Threading.Thread.Sleep(3000);

            // Verifică ordinea produselor după preț
            IList<IWebElement> productPrices = driver.FindElements(By.CssSelector(".inventory_item_price"));
            List<decimal> prices = new List<decimal>();

            foreach (var priceElement in productPrices)
            {
                string priceText = priceElement.Text.Replace("$", "");
                if (Decimal.TryParse(priceText, out decimal price))
                {
                    prices.Add(price);
                }
            }

            // Verifică dacă lista de prețuri este sortată în ordine crescătoare
            Assert.IsTrue(IsSortedAscending(prices), "Produsele nu sunt sortate corect după preț.");

        }
        [TestMethod]
        [TestCategory("Sortare")]
        public void TestSortareProduseDupaPretDescrescator()
        {
            // Deschide pagina de autentificare
            driver.Navigate().GoToUrl("https://www.saucedemo.com");
            driver.Manage().Window.Maximize();

            // Autentifică-te cu credențiale valide (adaptă acești pași în funcție de necesități)
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));

            usernameField.SendKeys("standard_user");
            System.Threading.Thread.Sleep(1000);
            passwordField.SendKeys("secret_sauce");
            System.Threading.Thread.Sleep(1000);
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);

            // Selectează opțiunea de sortare după preț descrescător
            IWebElement sortDropdown = driver.FindElement(By.ClassName("product_sort_container"));
            SelectElement selectSort = new SelectElement(sortDropdown);
            selectSort.SelectByText("Price (high to low)");
            System.Threading.Thread.Sleep(1000);

            // Așteaptă ca produsele să se sorteze
            System.Threading.Thread.Sleep(2000);

            // Verifică ordinea produselor după preț
            IList<IWebElement> productPrices = driver.FindElements(By.CssSelector(".inventory_item_price"));
            List<decimal> prices = new List<decimal>();

            foreach (var priceElement in productPrices)
            {
                string priceText = priceElement.Text.Replace("$", "");
                if (Decimal.TryParse(priceText, out decimal price))
                {
                    prices.Add(price);
                }
            }

            // Verifică dacă lista de prețuri este sortată în ordine descrescătoare
            Assert.IsTrue(IsSortedDescending(prices), "Produsele nu sunt sortate corect după preț descrescător.");

        }
        [TestMethod]
        [TestCategory("Sortare")]
        public void TestSortareProduseInOrdineAlfabetica()
        {
            // Deschide pagina de autentificare
            driver.Navigate().GoToUrl("https://www.saucedemo.com");
            driver.Manage().Window.Maximize();

            // Autentifică-te cu credențiale valide (adaptați acești pași în funcție de necesități)
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));

            usernameField.SendKeys("standard_user");
            System.Threading.Thread.Sleep(1000);
            passwordField.SendKeys("secret_sauce");
            System.Threading.Thread.Sleep(1000);
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);

            // Selectează opțiunea de sortare alfabetică
            IWebElement sortDropdown = driver.FindElement(By.ClassName("product_sort_container"));
            SelectElement selectSort = new SelectElement(sortDropdown);
            selectSort.SelectByText("Name (A to Z)");
            System.Threading.Thread.Sleep(1000);

            // Așteaptă ca produsele să se sorteze
            System.Threading.Thread.Sleep(2000);

            // Verifică ordinea produselor după nume
            IList<IWebElement> productNames = driver.FindElements(By.CssSelector(".inventory_item_name"));
            List<string> names = new List<string>();

            foreach (var nameElement in productNames)
            {
                names.Add(nameElement.Text);
            }

            // Verifică dacă lista de nume este sortată în ordine alfabetică
            Assert.IsTrue(IsSortedAlphabetically(names), "Produsele nu sunt sortate corect în ordine alfabetică.");

        }
        [TestMethod]
        [TestCategory("Sortare")]
        public void TestSortareProduseDupaNumeDescrescator()
        {
            // Deschide pagina de autentificare
            driver.Navigate().GoToUrl("https://www.saucedemo.com");
            driver.Manage().Window.Maximize();
            // Autentifică-te cu credențiale valide (adaptați acești pași în funcție de necesități)
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));
            usernameField.SendKeys("standard_user");
            System.Threading.Thread.Sleep(1000);
            passwordField.SendKeys("secret_sauce");
            System.Threading.Thread.Sleep(1000);
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);
            // Selectează opțiunea de sortare descrescătoare (de la Z la A)
            IWebElement sortDropdown = driver.FindElement(By.ClassName("product_sort_container"));
            SelectElement selectSort = new SelectElement(sortDropdown);
            selectSort.SelectByText("Name (Z to A)");
            System.Threading.Thread.Sleep(1000);
            // Așteaptă ca produsele să se sorteze
            System.Threading.Thread.Sleep(2000);
            // Verifică ordinea produselor după nume
            IList<IWebElement> productNames = driver.FindElements(By.CssSelector(".inventory_item_name"));
            List<string> names = new List<string>();

            foreach (var nameElement in productNames)
            {
                names.Add(nameElement.Text);
            }
            // Verifică dacă lista de nume este sortată în ordine descrescătoare
            Assert.IsTrue(IsSortedAlphabeticallyDescending(names), "Produsele nu sunt sortate corect în ordine descrescătoare.");

        }
        [TestMethod]
        [TestCategory("Cart")]
        public void AdaugareProdusInCos()
        {
            // Deschide pagina de autentificare
            driver.Navigate().GoToUrl("https://www.saucedemo.com");
            driver.Manage().Window.Maximize();
            // Autentifică-te cu credențiale valide (adaptați acești pași în funcție de necesități)
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));
            usernameField.SendKeys("standard_user");
            System.Threading.Thread.Sleep(1000);
            passwordField.SendKeys("secret_sauce");
            System.Threading.Thread.Sleep(1000);
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);
            System.Threading.Thread.Sleep(2000);
            // Adaugă primul produs în coș
            IWebElement addToCartButton = driver.FindElement(By.CssSelector(".btn_inventory"));
            addToCartButton.Click();
            System.Threading.Thread.Sleep(2000);
            // Accesează coșul pentru a verifica adăugarea produsului
            IWebElement cartIcon = driver.FindElement(By.CssSelector(".shopping_cart_container"));
            cartIcon.Click();
            System.Threading.Thread.Sleep(1000);
            // Verifică dacă produsul adăugat este prezent în coș
            IWebElement cartItem = driver.FindElement(By.CssSelector(".cart_item"));
            Assert.IsTrue(cartItem.Displayed, "Produsul nu a fost adăugat în coș.");
        }
        [TestMethod]
        [TestCategory("DetaliiProdus")]
        public void TestVerificareDetaliiProdus()
        {
            // Deschide pagina de autentificare
            driver.Navigate().GoToUrl("https://www.saucedemo.com");
            driver.Manage().Window.Maximize();
            // Autentifică-te cu credențiale valide (adaptați acești pași în funcție de necesități)
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));
            usernameField.SendKeys("standard_user");
            System.Threading.Thread.Sleep(1000);
            passwordField.SendKeys("secret_sauce");
            System.Threading.Thread.Sleep(1000);
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);
            // Accesează pagina cu produse
            IWebElement productsLink = driver.FindElement(By.CssSelector(".inventory_list"));
            productsLink.Click();
            System.Threading.Thread.Sleep(3000);
            // Accesează detaliile primului produs
            IWebElement firstProductLink = driver.FindElement(By.CssSelector(".inventory_item a"));
            firstProductLink.Click();
            System.Threading.Thread.Sleep(3000);
            // Verifică dacă sunteți pe pagina de detalii a produsului
            string productDetailsTitle = driver.FindElement(By.CssSelector(".inventory_details_name")).Text;
            Assert.IsTrue(productDetailsTitle.Length > 0, "Nu a fost accesată pagina de detalii a produsului.");
            // Reveniți la pagina cu lista de produse (opțional)
            driver.Navigate().Back();

        }
        [TestMethod]
        [TestCategory("DetaliiProdus")]
        public void TestVerificarePretProdus()
        {
            // Deschide pagina de autentificare
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.Manage().Window.Maximize();
            // Autentifică-te cu credențiale valide (adaptați acești pași în funcție de necesități)
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));
            usernameField.SendKeys("standard_user");
            System.Threading.Thread.Sleep(1000);
            passwordField.SendKeys("secret_sauce");
            System.Threading.Thread.Sleep(1000);
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);
            // Accesează pagina cu produse
            IWebElement productsLink = driver.FindElement(By.CssSelector(".inventory_list"));
            productsLink.Click();
            System.Threading.Thread.Sleep(3000);
            // Alege un produs din listă (aici se alege primul produs)
            IWebElement productLink = driver.FindElement(By.CssSelector(".inventory_item a"));
            string productName = productLink.Text;
            productLink.Click();
            System.Threading.Thread.Sleep(3000);
            // Obține prețul produsului
            IWebElement productPriceElement = driver.FindElement(By.CssSelector(".inventory_details_price"));
            string productPriceText = productPriceElement.Text;
            // Convertește prețul din format text la tipul de date corespunzător (de exemplu, decimal)
            if (decimal.TryParse(productPriceText.Replace("$", ""), out decimal productPrice))
            {
                // Verifică dacă prețul produsului este corect 
                decimal expectedPrice = 29.99M; // Actualizați cu prețul așteptat
                Assert.AreEqual(expectedPrice, productPrice, $"Prețul produsului {productName} nu este corect.");
            }
            else
            {
                Assert.Fail($"Nu s-a putut converti prețul produsului {productName} într-un număr valid.");
            }
        }
        [TestMethod]
        [TestCategory("DetaliiProdus")]
        public void TestVerificareDescriereProdus()
        {
            // Deschide pagina de autentificare
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.Manage().Window.Maximize();
            // Autentifică-te cu credențiale valide (adaptați acești pași în funcție de necesități)
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));
            usernameField.SendKeys("standard_user");
            System.Threading.Thread.Sleep(1000);
            passwordField.SendKeys("secret_sauce");
            System.Threading.Thread.Sleep(1000);
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);
            // Accesează pagina cu produse
            IWebElement productsLink = driver.FindElement(By.CssSelector(".inventory_list"));
            productsLink.Click();
            System.Threading.Thread.Sleep(3000);
            // Alege un produs din listă (aici se alege primul produs)
            IWebElement productLink = driver.FindElement(By.CssSelector(".inventory_item a"));
            string productName = productLink.Text;
            productLink.Click();
            // Obține descrierea produsului
            IWebElement productDescriptionElement = driver.FindElement(By.CssSelector(".inventory_details_desc"));
            string productDescription = productDescriptionElement.Text;
            // Verifică dacă descrierea produsului este neagră și conține informații relevante
            Assert.IsTrue(!string.IsNullOrEmpty(productDescription), $"Descrierea produsului {productName} este goală sau inexistentă.");
        }
        [TestMethod]
        [TestCategory("Cart")]
        public void TestStergereElementDinCos()
        {
            // Deschide pagina de autentificare
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.Manage().Window.Maximize();

            // Autentifică-te cu credențiale valide (adaptați acești pași în funcție de necesități)
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));

            usernameField.SendKeys("standard_user");
            System.Threading.Thread.Sleep(1000);
            passwordField.SendKeys("secret_sauce");
            System.Threading.Thread.Sleep(1000);
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);
            // Accesează pagina cu produse
            IWebElement productsLink = driver.FindElement(By.CssSelector(".inventory_list"));
            productsLink.Click();
            // Adaugă un produs în coș (aici se adaugă primul produs)
            IWebElement addToCartButton = driver.FindElement(By.CssSelector(".btn_inventory"));
            addToCartButton.Click();
            System.Threading.Thread.Sleep(1000);
            // Accesează coșul pentru a verifica adăugarea produsului
            IWebElement cartIcon = driver.FindElement(By.CssSelector(".shopping_cart_container"));
            cartIcon.Click();
            System.Threading.Thread.Sleep(1000);
            // Verifică dacă produsul adăugat este prezent în coș
            IWebElement cartItem = driver.FindElement(By.CssSelector(".cart_item"));
            Assert.IsTrue(cartItem.Displayed, "Produsul nu a fost adăugat în coș.");
            // Șterge produsul din coș
            IWebElement removeButton = driver.FindElement(By.CssSelector("button.btn_secondary.cart_button"));
            removeButton.Click();
            System.Threading.Thread.Sleep(1000);
            // Așteaptă puțin pentru a permite coșului să se actualizeze
            System.Threading.Thread.Sleep(2000);
            // Verifică dacă produsul a fost șters din coș
            try
            {
                // Încercă să găsești elementul șters
                IWebElement removedItem = driver.FindElement(By.CssSelector(".cart_item"));
                Assert.Fail("Produsul nu a fost șters din coș.");
            }
            catch (NoSuchElementException)
            {
                // Elementul nu a fost găsit, ceea ce înseamnă că a fost șters corect
            }
        }
        [TestMethod]
        [TestCategory("Logout")]
        public void TestDelogareSauceDemo()
        {
            // Deschide pagina de autentificare
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.Manage().Window.Maximize();
            // Autentifică-te cu credențiale valide (adaptați acești pași în funcție de necesități)
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));
            usernameField.SendKeys("standard_user");
            System.Threading.Thread.Sleep(1000);
            passwordField.SendKeys("secret_sauce");
            System.Threading.Thread.Sleep(1000);
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);
            // Așteaptă ca pagina să se încarce
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("/inventory.html"));
            // Verifică dacă ești pe pagina de produse
            Assert.IsTrue(driver.Url.Contains("/inventory.html"), "Nu ești pe pagina corectă cu produse.");
            // Delogare
            IWebElement logoutButton = driver.FindElement(By.CssSelector(".bm-burger-button"));
            logoutButton.Click();
            System.Threading.Thread.Sleep(3000);
            IWebElement logoutLink = driver.FindElement(By.Id("logout_sidebar_link"));
            logoutLink.Click();
            // Așteaptă ca pagina să se încarce
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("https://www.saucedemo.com/"));
            // Verifică dacă ești delogat și revenit la pagina de autentificare
            Assert.IsTrue(driver.Url.Equals("https://www.saucedemo.com/"), "Nu ești delogat și revenit la pagina de autentificare.");
        }
        [TestMethod]
        [TestCategory("Navigare")]
        public void TestNavigarePaginaAbout()
        {
            // Deschideți pagina principală a SauceDemo
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.Manage().Window.Maximize();
            // Autentifică-te cu credențiale valide (adaptați acești pași în funcție de necesități)
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));
            usernameField.SendKeys("standard_user");
            System.Threading.Thread.Sleep(1000);
            passwordField.SendKeys("secret_sauce");
            System.Threading.Thread.Sleep(1000);
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);
            // Așteaptă ca pagina să se încarce complet
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.UrlContains("/inventory.html"));
            IWebElement moreButton = driver.FindElement(By.CssSelector(".bm-burger-button"));
            moreButton.Click();
            System.Threading.Thread.Sleep(3000);
            // Identificați și faceți clic pe link-ul către pagina "About"
            IWebElement aboutLink = driver.FindElement(By.Id("about_sidebar_link"));
            aboutLink.Click();
            // Așteaptă ca pagina "About" să se încarce complet
            wait.Until(ExpectedConditions.UrlContains("https://saucelabs.com/"));
            // Verifică dacă ești pe pagina "About"
            Assert.IsTrue(driver.Url.Contains("https://saucelabs.com/"), "Nu ești pe pagina corectă 'About'.");
        }
        [TestMethod]
        [TestCategory("Cart")]
        public void TestCumparareProdus()
        {
            // Deschideți pagina principală a SauceDemo
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.Manage().Window.Maximize();
            // Autentifică-te cu credențiale valide (adaptați acești pași în funcție de necesități)
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));
            usernameField.SendKeys("standard_user");
            System.Threading.Thread.Sleep(1000);
            passwordField.SendKeys("secret_sauce");
            System.Threading.Thread.Sleep(1000);
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);
            // Așteaptă ca pagina să se încarce complet
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.UrlContains("/inventory.html"));
            // Adăugați un produs în coș (puteți adapta în funcție de specificul site-ului)
            IWebElement addToCartButton = driver.FindElement(By.CssSelector(".btn_inventory"));
            addToCartButton.Click();
            // Așteaptă ca produsul să fie adăugat în coș
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".shopping_cart_badge")));
            // Navigați la coș
            IWebElement cartIcon = driver.FindElement(By.CssSelector(".shopping_cart_container"));
            cartIcon.Click();
            // Așteaptă ca pagina coșului să se încarce complet
            wait.Until(ExpectedConditions.UrlContains("/cart.html"));
            // Faceți clic pe butonul "Checkout"
            IWebElement checkoutButton = driver.FindElement(By.CssSelector(".checkout_button"));
            checkoutButton.Click();
            // Așteaptă ca pagina de informații de livrare să se încarce complet
            wait.Until(ExpectedConditions.UrlContains("/checkout-step-one.html"));

            // Verifică dacă ești pe pagina corectă (de exemplu, pagina de informații de livrare)
            Assert.IsTrue(driver.Url.Contains("/checkout-step-one.html"), "Nu ești pe pagina corectă pentru informații de livrare.");
        }
        [TestMethod]
        [TestCategory("Cart")]
        public void TestCheckOutComplet()
        {
            // Deschideți pagina principală a SauceDemo
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.Manage().Window.Maximize();

            // Autentifică-te cu credențiale valide (adaptați acești pași în funcție de necesități)
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));

            usernameField.SendKeys("standard_user");
            System.Threading.Thread.Sleep(1000);
            passwordField.SendKeys("secret_sauce");
            System.Threading.Thread.Sleep(1000);
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);
            System.Threading.Thread.Sleep(3000);
            // Așteaptă ca pagina să se încarce complet
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.UrlContains("/inventory.html"));
            System.Threading.Thread.Sleep(3000);
            // Adăugați un produs în coș 
            IWebElement addToCartButton = driver.FindElement(By.CssSelector(".btn_inventory"));
            addToCartButton.Click();
            System.Threading.Thread.Sleep(3000);
            // Așteaptă ca produsul să fie adăugat în coș
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".shopping_cart_badge")));
            System.Threading.Thread.Sleep(3000);
            // Navigați la coș
            IWebElement cartIcon = driver.FindElement(By.CssSelector(".shopping_cart_container"));
            cartIcon.Click();
            System.Threading.Thread.Sleep(3000);
            // Așteaptă ca pagina coșului să se încarce complet
            wait.Until(ExpectedConditions.UrlContains("/cart.html"));
            System.Threading.Thread.Sleep(3000);
            //Faceți clic pe butonul "Checkout"
            IWebElement checkoutButton = driver.FindElement(By.CssSelector(".checkout_button"));
            checkoutButton.Click();
            System.Threading.Thread.Sleep(3000);
            // Așteaptă ca pagina de informații de livrare să se încarce complet
            wait.Until(ExpectedConditions.UrlContains("/checkout-step-one.html"));
            System.Threading.Thread.Sleep(3000);
            // Completați informațiile de livrare
            IWebElement firstNameInput = driver.FindElement(By.Id("first-name"));
            firstNameInput.SendKeys("Laura");
            System.Threading.Thread.Sleep(3000);
            IWebElement lastNameInput = driver.FindElement(By.Id("last-name"));
            lastNameInput.SendKeys("Pop");
            System.Threading.Thread.Sleep(3000);
            IWebElement zipCodeInput = driver.FindElement(By.Id("postal-code"));
            zipCodeInput.SendKeys("1234");
            System.Threading.Thread.Sleep(3000);
            // Faceți clic pe butonul "Continue"
            IWebElement continueButton = driver.FindElement(By.CssSelector(".btn_primary"));
            continueButton.Click();
            System.Threading.Thread.Sleep(3000);
            // Așteaptă ca pagina de informații de plata să se încarce complet
            wait.Until(ExpectedConditions.UrlContains("/checkout-step-two.html"));
            System.Threading.Thread.Sleep(3000);
            // Faceți clic pe butonul "Finish"
            IWebElement finishButton = driver.FindElement(By.CssSelector(".btn_action"));
            finishButton.Click();
            System.Threading.Thread.Sleep(3000);
            // Așteaptă ca pagina de confirmare a comenzii să se încarce complet
            wait.Until(ExpectedConditions.UrlContains("/checkout-complete.html"));
            // Verifică dacă ești pe pagina corectă (de exemplu, pagina de confirmare a comenzii)
            Assert.IsTrue(driver.Url.Contains("/checkout-complete.html"), "Nu ești pe pagina corectă pentru confirmarea comenzii.");
        }
        [TestMethod]
        [TestCategory("DetaliiProdus")]
        public void TestVerificareImagineProdus()
        {
            // Deschideți pagina principală a SauceDemo
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.Manage().Window.Maximize();

            // Autentifică-te cu credențiale valide (adaptați acești pași în funcție de necesități)
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));

            usernameField.SendKeys("standard_user");
            System.Threading.Thread.Sleep(1000);
            passwordField.SendKeys("secret_sauce");
            System.Threading.Thread.Sleep(1000);
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);

            // Așteaptă ca pagina să se încarce complet
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.UrlContains("/inventory.html"));

            // Identificați elementul care conține imaginea produsului (exemplu: primul produs în listă)
            IWebElement productImage = driver.FindElement(By.CssSelector(".inventory_item_img"));

            // Verifică dacă imaginea este prezentă
            Assert.IsTrue(productImage.Displayed, "Imaginea produsului nu este afișată.");

        }
        [TestMethod]
        [TestCategory("DetaliiProdus")]
        public void TestVerificareTitluPagina()
        {
            // Deschideți pagina principală a SauceDemo
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.Manage().Window.Maximize();
            // Autentifică-te cu credențiale valide (adaptați acești pași în funcție de necesități)
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));
            usernameField.SendKeys("standard_user");
            System.Threading.Thread.Sleep(1000);
            passwordField.SendKeys("secret_sauce");
            System.Threading.Thread.Sleep(1000);
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);
            // Așteaptă ca pagina să se încarce complet
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.UrlContains("/inventory.html"));
            // Obțineți titlul paginii
            string pageTitle = driver.Title;
            // Verifică dacă titlul este cel așteptat
            Assert.AreEqual("Swag Labs", pageTitle, "Titlul paginii nu este cel așteptat.");
        }
        [TestMethod]
        [TestCategory("Cart")]
        public void TestFinalizareComandaFaraInformatiiLivrare()
        {
            // Deschideți pagina principală a SauceDemo și adăugați produse în coș
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.Manage().Window.Maximize();

            // Autentifică-te cu credențiale valide (adaptați acești pași în funcție de necesități)
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));

            usernameField.SendKeys("standard_user");
            System.Threading.Thread.Sleep(1000);
            passwordField.SendKeys("secret_sauce");
            System.Threading.Thread.Sleep(1000);
            loginButton.Click();
            System.Threading.Thread.Sleep(1000);

            // Așteaptă ca pagina să se încarce complet
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.UrlContains("/inventory.html"));
            System.Threading.Thread.Sleep(3000);

            // Adăugați un produs în coș (puteți adapta în funcție de specificul site-ului)
            IWebElement addToCartButton = driver.FindElement(By.CssSelector(".btn_inventory"));
            addToCartButton.Click();

            System.Threading.Thread.Sleep(3000);

            // Așteaptă ca produsul să fie adăugat în coș
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".shopping_cart_badge")));

            System.Threading.Thread.Sleep(3000);
            // Navigați la coș
            IWebElement cartIcon = driver.FindElement(By.CssSelector(".shopping_cart_container"));
            cartIcon.Click();

            // Așteaptă ca pagina coșului să se încarce complet
            wait.Until(ExpectedConditions.UrlContains("/cart.html"));
            System.Threading.Thread.Sleep(3000);

            // Continuați cu operațiunile de plată fără a completa informațiile de livrare
            IWebElement checkoutButton = driver.FindElement(By.CssSelector(".checkout_button"));
            checkoutButton.Click();
            System.Threading.Thread.Sleep(3000);

            // Așteaptă ca pagina de informații de livrare să se încarce complet
            wait.Until(ExpectedConditions.UrlContains("/checkout-step-one.html"));
            System.Threading.Thread.Sleep(3000);

            // Faceți clic pe butonul "Finish" pentru a încerca să finalizați comanda
            IWebElement finishButton = driver.FindElement(By.CssSelector(".btn_primary"));
            finishButton.Click();

            // Verificați dacă aplicația afișează mesaje de eroare pentru câmpurile necompletate
            IWebElement errorMessage = driver.FindElement(By.CssSelector("button.error-button"));
            Assert.IsTrue(errorMessage.Displayed, "Mesajul de eroare pentru informații de livrare incomplete nu este afișat corespunzător.");

            // Puteți adăuga și alți pași de verificare sau de manipulare în funcție de necesități
        }
        [TestMethod]
        [TestCategory("Performance")]
        public void TestTimpIncarcarePaginaPrincipala()
        {
            Stopwatch stopwatch = new Stopwatch();

            // Măsoară timpul de încărcare a paginii principale
            stopwatch.Start();
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            stopwatch.Stop();

            // Afișează timpul de încărcare
            Console.WriteLine($"Timpul de încărcare a paginii principale: {stopwatch.ElapsedMilliseconds} milisecunde");


        }
        [TestMethod]
        [TestCategory("Performance")]
        public void TestTimpAdaugareProdusInCos()
        {
            // Deschideți pagina principală a SauceDemo și adăugați produse în coș
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.Manage().Window.Maximize();
            // Autentifică-te cu credențiale valide (adaptați acești pași în funcție de necesități)
            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));
            usernameField.SendKeys("standard_user");
            passwordField.SendKeys("secret_sauce");
            loginButton.Click();
            // Așteaptă ca pagina să se încarce complet
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.UrlContains("/inventory.html"));
            // Măsoară timpul necesar pentru a adăuga un produs în coș
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            // Adăugați un produs în coș (puteți adapta în funcție de specificul site-ului)
            IWebElement addToCartButton = driver.FindElement(By.CssSelector(".btn_inventory"));
            addToCartButton.Click();
            // Așteaptă ca produsul să fie adăugat în coș
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".shopping_cart_badge")));
            // Oprirea ceasului dupa adaugarea produsului in cos
            stopwatch.Stop();
            // Afișează timpul de adăugare în coș
            Console.WriteLine($"Timpul necesar pentru a adăuga un produs în coș: {stopwatch.ElapsedMilliseconds} milisecunde");
        }
        [TestCleanup]
        public void Teardown()
        {
            System.Threading.Thread.Sleep(2000);
            driver.Quit();
        }
        private bool IsSortedAscending(List<decimal> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i] > list[i + 1])
                {
                    return false;
                }
            }
            return true;
        }
        private bool IsSortedDescending(List<decimal> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i] < list[i + 1])
                {
                    return false;
                }
            }
            return true;
        }
        private bool IsSortedAlphabetically(List<string> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (string.Compare(list[i], list[i + 1], StringComparison.Ordinal) > 0)
                {
                    return false;
                }
            }
            return true;
        }
        private bool IsSortedAlphabeticallyDescending(List<string> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (string.Compare(list[i], list[i + 1], StringComparison.Ordinal) < 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
