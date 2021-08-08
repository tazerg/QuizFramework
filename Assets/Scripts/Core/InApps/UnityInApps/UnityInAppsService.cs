using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using Zenject;

namespace QuizFramework.InApps
{
    public class UnityInAppsService : IInAppsService, IStoreListener, IInitializable
    {
        private readonly IInAppsConfig _inAppsConfig;
        
        private IStoreController _storeController;
        private IExtensionProvider _extensionsProvider;

        private bool _isInitialized;

        public UnityInAppsService(IInAppsConfig inAppsConfig)
        {
            _inAppsConfig = inAppsConfig;
        }

        private void Initialize()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            var products = CollectInAppProducts();
            builder.AddProducts(products);
            
            UnityPurchasing.Initialize(this, builder);
        }

        private IEnumerable<ProductDefinition> CollectInAppProducts()
        {
            var result = new List<ProductDefinition>();
            var inGameInApps = _inAppsConfig.GetInAppInfos();
            foreach (var inGameInApp in inGameInApps)
            {
                var convertedInAppType = ConvertInAppType(inGameInApp.Type);
                var product = new ProductDefinition(inGameInApp.Id, convertedInAppType);
                result.Add(product);
            }

            return result;
        }

        private void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _isInitialized = true;
            _storeController = controller;
            _extensionsProvider = extensions;
        }

        private void OnInitializeFailed(InitializationFailureReason error)
        {
            _isInitialized = false;
            Debug.LogError($"UnityInAppsService initialization failed. Reason {error}");
        }
        
        private void PurchaseProduct(string productId)
        {
            if (!_isInitialized)
            {
                Debug.LogError("Purchasing service not initialized!");
                return;
            }
            
            _storeController.InitiatePurchase(productId);
        }

        private PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            Debug.Log($"Purchase complete {purchaseEvent.purchasedProduct.definition.id}");
            return PurchaseProcessingResult.Complete;
        }

        private void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.LogError($"Purchase in-app failed {product.definition.id} {failureReason}");
        }

        private ProductType ConvertInAppType(InAppType inAppType)
        {
            switch (inAppType)
            {
                case InAppType.Consumable:
                    return ProductType.Consumable;
                case InAppType.NonConsumable:
                    return ProductType.NonConsumable;
                case InAppType.Subscription:
                    return ProductType.Subscription;
                default:
                    throw new ArgumentException($"Not supported in-app type {inAppType}");
            }
        }

        #region IInAppsService

        bool IInAppsService.IsInitialized => _isInitialized;
        
        void IInAppsService.PurchaseProduct(string productId)
        {
            PurchaseProduct(productId);
        }

        #endregion

        #region IStoreListener

        void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            OnInitialized(controller, extensions);
        }

        void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
        {
            OnInitializeFailed(error);
        }

        PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            return ProcessPurchase(purchaseEvent);
        }

        void IStoreListener.OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            OnPurchaseFailed(product, failureReason);
        }

        #endregion

        #region IInitializable

        void IInitializable.Initialize()
        {
            Initialize();
        }

        #endregion
    }
}