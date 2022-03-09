using Moq;
using Solnet.Rpc;
using Solnet.Rpc.Core.Http;
using Solnet.Rpc.Messages;
using Solnet.Rpc.Models;
using Solnet.Rpc.Types;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Solnet.Solend.Test
{
    public abstract class BaseTest
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        /// <summary>
        /// Setup the JSON RPC test with the request and response data.
        /// </summary>
        /// <param name="responseContent">The response content.</param>
        /// <param name="address">The address parameter for <c>GetAccountInfo</c>.</param>
        /// <param name="commitment">The commitment parameter for the <c>GetAccountInfo</c>.</param>
        /// <param name="network">The network address for the <c>GetAccountInfo</c> request.</param>
        protected static Mock<IRpcClient> SetupGetAccountInfo(string responseContent, string address, string network,
            Commitment commitment = Commitment.Finalized)
        {
            var rpcMock = new Mock<IRpcClient>(MockBehavior.Strict) { };
            rpcMock
                .Setup(s => s.NodeAddress)
                .Returns(new Uri(network))
                .Verifiable();
            rpcMock
                .Setup(s => s.GetAccountInfoAsync(
                        It.Is<string>(s1 => s1 == address),
                        It.Is<Commitment>(c => c == commitment),
                        It.IsAny<BinaryEncoding>()))
                .ReturnsAsync(() =>
                {
                    var res = new RequestResult<ResponseValue<AccountInfo>>(
                        new HttpResponseMessage(HttpStatusCode.OK),
                        JsonSerializer.Deserialize<ResponseValue<AccountInfo>>(responseContent, JsonSerializerOptions))
                    {
                        WasRequestSuccessfullyHandled = true
                    };

                    return res;
                })
                .Verifiable();
            return rpcMock;
        }

        /// <summary>
        /// Setup the JSON RPC test with the request and response data.
        /// </summary>
        /// <param name="responseContent">The response content.</param>
        /// <param name="commitment">The commitment parameter for the <c>GetProgramAccounts</c> request.</param>
        /// <param name="network">The network address for the <c>GetProgramAccounts</c> request.</param>
        protected static Mock<IRpcClient> SetupGetProgramAccounts(string responseContent, string network, string ownerAddress,
            int layoutLength, Commitment commitment = Commitment.Finalized)
        {
            var rpcMock = new Mock<IRpcClient>(MockBehavior.Strict) { };
            rpcMock
                .Setup(s => s.NodeAddress)
                .Returns(new Uri(network))
                .Verifiable();
            rpcMock
                .Setup(s => s.GetProgramAccountsAsync(
                    It.Is<string>(s1 => s1 == ownerAddress),
                    It.Is<Commitment>(c => c == commitment),
                    It.Is<int?>(i => i.Value == layoutLength),
                    It.IsAny<List<MemCmp>>()))
                .ReturnsAsync(() =>
                {
                    var res = new RequestResult<List<AccountKeyPair>>(
                        new HttpResponseMessage(HttpStatusCode.OK),
                        JsonSerializer.Deserialize<List<AccountKeyPair>>(responseContent, JsonSerializerOptions))
                    {
                        WasRequestSuccessfullyHandled = true
                    };

                    return res;
                })
                .Verifiable();
            return rpcMock;
        }

        /// <summary>
        /// Setup the JSON RPC test with the request and response data.
        /// </summary>
        /// <param name="responseContent">The response content.</param>
        /// <param name="commitment">The commitment parameter for the <c>GetProgramAccounts</c> request.</param>
        /// <param name="network">The network address for the <c>GetProgramAccounts</c> request.</param>
        protected static Mock<IRpcClient> SetupGetProgramAccountsFiltered(string responseContent, string network, string ownerAddress,
            int layoutLength, int filterOffset, string filterAddress, Commitment commitment = Commitment.Finalized)
        {
            var rpcMock = new Mock<IRpcClient>(MockBehavior.Strict) { };
            rpcMock
                .Setup(s => s.NodeAddress)
                .Returns(new Uri(network))
                .Verifiable();
            rpcMock
                .Setup(s => s.GetProgramAccountsAsync(
                    It.Is<string>(s1 => s1 == ownerAddress),
                    It.Is<Commitment>(c => c == commitment),
                    It.Is<int?>(i => i.Value == layoutLength),
                    It.Is<List<MemCmp>>(filters => filters.Find(cmp => cmp.Offset == filterOffset).Bytes == filterAddress)))
                .ReturnsAsync(() =>
                {
                    var res = new RequestResult<List<AccountKeyPair>>(
                        new HttpResponseMessage(HttpStatusCode.OK),
                        JsonSerializer.Deserialize<List<AccountKeyPair>>(responseContent, JsonSerializerOptions))
                    {
                        WasRequestSuccessfullyHandled = true
                    };

                    return res;
                })
                .Verifiable();
            return rpcMock;
        }

        /// <summary>
        /// Setup the JSON RPC test with the request and response data.
        /// </summary>
        /// <param name="responseContent">The response content.</param>
        /// <param name="commitment">The commitment parameter for the <c>GetProgramAccounts</c> request.</param>
        /// <param name="network">The network address for the <c>GetProgramAccounts</c> request.</param>
        protected static void SetupGetProgramAccountsFiltered(Mock<IRpcClient> rpcMock, string responseContent, string network, string ownerAddress,
            int layoutLength, int filterOffset, string filterAddress, Commitment commitment = Commitment.Finalized)
        {
            rpcMock
                .Setup(s => s.GetProgramAccountsAsync(
                    It.Is<string>(s1 => s1 == ownerAddress),
                    It.Is<Commitment>(c => c == commitment),
                    It.Is<int?>(i => i.Value == layoutLength),
                    It.Is<List<MemCmp>>(filters => filters.Find(cmp => cmp.Offset == filterOffset).Bytes == filterAddress)))
                .ReturnsAsync(() =>
                {
                    var res = new RequestResult<List<AccountKeyPair>>(
                        new HttpResponseMessage(HttpStatusCode.OK),
                        JsonSerializer.Deserialize<List<AccountKeyPair>>(responseContent, JsonSerializerOptions))
                    {
                        WasRequestSuccessfullyHandled = true
                    };

                    return res;
                })
                .Verifiable();
        }
    }
}
