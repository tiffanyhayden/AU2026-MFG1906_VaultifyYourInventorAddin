using System.Runtime.Versioning;
using Connectivity.InventorAddin.EdmAddin;
using VDF = Autodesk.DataManagement.Client.Framework;

namespace VaultifyYourInventorAddin
{
    [SupportedOSPlatform("windows7.0")]
    public static class VaultConn
    {
        /// <summary>
        /// Gets the active Vault connection if it exists and is connected. Returns null if no active connection or if the connection
        /// is not currently connected.
        /// </summary>
        /// <returns></returns>
        public static VDF.Vault.Currency.Connections.Connection GetActiveConnection()
        {
            EdmSecurity EDMS = EdmSecurity.Instance;
            VDF.Vault.Currency.Connections.Connection connection = EDMS.VaultConnection;

            if (connection != null && connection.IsConnected)
            {
                return connection;
            }
            return null;
        }

        /// <summary>
        /// Initializes the Vault connection if not already established.
        /// </summary>
        /// <returns>The active Vault connection or null if not connected.</returns>
        public static VDF.Vault.Currency.Connections.Connection InitializeConnection()
        {
            EdmSecurity EDMS = EdmSecurity.Instance;
            VDF.Vault.Currency.Connections.Connection connection = EDMS.VaultConnection;

            if (connection != null)
            {
                return connection;
            }

            return null;
        }
    }
}
