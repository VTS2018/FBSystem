﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18408
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FBSystem.MyPageCode {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MyPageCode.APISoap")]
    public interface APISoap {
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 url 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetPageCode", ReplyAction="*")]
        FBSystem.MyPageCode.GetPageCodeResponse GetPageCode(FBSystem.MyPageCode.GetPageCodeRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetPageCodeRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetPageCode", Namespace="http://tempuri.org/", Order=0)]
        public FBSystem.MyPageCode.GetPageCodeRequestBody Body;
        
        public GetPageCodeRequest() {
        }
        
        public GetPageCodeRequest(FBSystem.MyPageCode.GetPageCodeRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetPageCodeRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string url;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string signInfo;
        
        public GetPageCodeRequestBody() {
        }
        
        public GetPageCodeRequestBody(string url, string signInfo) {
            this.url = url;
            this.signInfo = signInfo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetPageCodeResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetPageCodeResponse", Namespace="http://tempuri.org/", Order=0)]
        public FBSystem.MyPageCode.GetPageCodeResponseBody Body;
        
        public GetPageCodeResponse() {
        }
        
        public GetPageCodeResponse(FBSystem.MyPageCode.GetPageCodeResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetPageCodeResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetPageCodeResult;
        
        public GetPageCodeResponseBody() {
        }
        
        public GetPageCodeResponseBody(string GetPageCodeResult) {
            this.GetPageCodeResult = GetPageCodeResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface APISoapChannel : FBSystem.MyPageCode.APISoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class APISoapClient : System.ServiceModel.ClientBase<FBSystem.MyPageCode.APISoap>, FBSystem.MyPageCode.APISoap {
        
        public APISoapClient() {
        }
        
        public APISoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public APISoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public APISoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public APISoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        FBSystem.MyPageCode.GetPageCodeResponse FBSystem.MyPageCode.APISoap.GetPageCode(FBSystem.MyPageCode.GetPageCodeRequest request) {
            return base.Channel.GetPageCode(request);
        }
        
        public string GetPageCode(string url, string signInfo) {
            FBSystem.MyPageCode.GetPageCodeRequest inValue = new FBSystem.MyPageCode.GetPageCodeRequest();
            inValue.Body = new FBSystem.MyPageCode.GetPageCodeRequestBody();
            inValue.Body.url = url;
            inValue.Body.signInfo = signInfo;
            FBSystem.MyPageCode.GetPageCodeResponse retVal = ((FBSystem.MyPageCode.APISoap)(this)).GetPageCode(inValue);
            return retVal.Body.GetPageCodeResult;
        }
    }
}