using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApi.Configure
{
    /// <summary>
    /// 全局配置route
    /// </summary>
    public class RouteConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _attributeRouteModel;
        public RouteConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            _attributeRouteModel = new AttributeRouteModel(routeTemplateProvider);
        }
        public void Apply(ApplicationModel application)
        {
            IList<ControllerModel> controllers = application.Controllers;
            foreach (var controller in controllers)
            {
                // 标记 RouteAttribute 的 Controllers
                var controllerSelectors = controller.Selectors.Where(s => s.AttributeRouteModel != null).ToList();
                if (controllerSelectors.Any())
                {
                    for (int i = 0; i < controllerSelectors.Count;i++)
                    {
                        controllerSelectors[i].AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_attributeRouteModel, controllerSelectors[i].AttributeRouteModel);
                    }
                }
                // 没有标记 RouteAttribute 的 Controllers
                var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();
                if (unmatchedSelectors.Any())
                {
                    for (int i = 0; i < controllerSelectors.Count; i++)
                    {
                        // 添加一个 路由前缀
                        controllerSelectors[i].AttributeRouteModel = _attributeRouteModel;
                    }
                }
            }
        }
        public static void UseCentralRoutePrefix(MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            // 添加我们自定义 实现IApplicationModelConvention的RouteConvention
            opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
        }
    }
}
