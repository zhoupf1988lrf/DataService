using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core31DemoProject.Controllers
{
    /// <summary>
    /// [ApiController]属性将推理规则应用于操作参数的默认数据源。借助这些规则，无需将属性应用于操作参数来手动识别绑定源。绑定源的推理规则的行为如下：
    /// 1 [FromBody]针对复杂类型的参数进行推断.[FromBody]不适用于具有特殊含义的任何复杂的内置类型，如IFormCollection、CancellationToken
    /// 2 [FromForm]针对IFormFile、IFormFileCollection类型的参数进行推断。该特性不针对任何简单类型或用户自定义的类型进行推断
    /// 3 [FromRoute]针对与路由模板中的参数相匹配的任何参数名称进行推断。当多个路由与一个操作参数匹配时，任何路由值都是为[FromRoute]
    /// 4 [FromQuery]针对任何其他操作参数进行推断
    /// 
    /// 所以ApiController主要是做 参数 数据源推理，有一套自己的推理规则，规则如上
    /// </summary>
    [Route("api/[controller]/[action]"),ApiController]
    public class ApiBaseController : ControllerBase
    {
    }
}
