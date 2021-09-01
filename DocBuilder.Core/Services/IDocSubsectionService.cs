using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocBuilder.Core.Services
{
    interface IDocSubsectionService
    {
        /// <summary>
        /// Удаляет секции в документе, которые окруженны комментариями.
        /// Оставляет только те секции с комментариями, которые упомянуты в
        /// файле ответов пакета в разделе packItems[].Variants[]
        /// </summary>
        /// <param name="filePath"></param>
        public void RemoveNeedlessSubsectionsFrom(string filePath);
    }
}
