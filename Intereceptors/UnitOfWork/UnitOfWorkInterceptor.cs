using System;
using Core;
using Core.Abstract;
using Core.Attributes;

namespace Intereceptors.UnitOfWork
{
    public class UnitOfWorkInterceptor: MethodBoundaryInterceptor
    {
        private Lazy<IUnitOfWork> _unitOfWork;
        private UnitOfWorkAttribute _attribute;

        public UnitOfWorkInterceptor(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWork = new Lazy<IUnitOfWork>(unitOfWorkFactory.Create);
        }

        protected override void BeforeInvoke(InvocationContext invocationContext)
        {
            _attribute = invocationContext.GetOwningAttribute() as UnitOfWorkAttribute;

            var uowParamPosition = invocationContext.GetParameterPosition<IUnitOfWork>();
            var uow = invocationContext.GetParameterValue<IUnitOfWork>(uowParamPosition);

            if(uow != null) return;

            invocationContext.SetParameterValue(uowParamPosition, _unitOfWork.Value);
        }
        
        protected override void AfterInvoke(InvocationContext invocationContext, object returnValue)
        {
            if (!_attribute.SaveChanges)
                return;

            _unitOfWork.Value.SaveChanges();
        }
    }
}