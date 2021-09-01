using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocBuilder.Core
{
    interface IDocPropertyService
    {
        /// <summary>
        /// Подставляет в документ все поля (value) из generalDocProperties
        /// </summary>
        /// <param name="filePath"></param>
        public void ReplaceGeneralPropsIn(string path);

        /// <summary>
        /// Если есть paсkItem[].DockProperties для данного документа,
        /// то подставляет в документ все поля (value) из paсkItem[].DockProperties
        /// </summary>
        /// <param name="filePath"></param>
        public void ReplacePackItemPropsIn(string path);
    }
}
