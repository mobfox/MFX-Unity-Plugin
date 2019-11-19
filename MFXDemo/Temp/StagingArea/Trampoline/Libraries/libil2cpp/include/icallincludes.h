#include "icalls/mscorlib/Mono.Globalization.Unicode/Normalization.h"

#include "icalls/mscorlib/Mono.Interop/ComInteropProxy.h"

#include "icalls/mscorlib/Mono/Runtime.h"

#include "icalls/mscorlib/Mono.Security.Cryptography/KeyPairPersistence.h"

#include "icalls/mscorlib/Mono.Unity/UnityTls.h"

#include "icalls/mscorlib/System/__ComObject.h"

#include "icalls/mscorlib/System/Activator.h"
#include "icalls/mscorlib/System/AppDomain.h"
#include "icalls/mscorlib/System/ArgIterator.h"
#include "icalls/mscorlib/System/Array.h"
#include "icalls/mscorlib/System/Buffer.h"
#include "icalls/mscorlib/System/Char.h"
#include "icalls/mscorlib/System/CLRConfig.h"
#include "icalls/mscorlib/System/Convert.h"
#include "icalls/mscorlib/System/ConsoleDriver.h"
#include "icalls/mscorlib/System/CurrentSystemTimeZone.h"
#include "icalls/mscorlib/System/DateTime.h"
#include "icalls/mscorlib/System/Decimal.h"
#include "icalls/mscorlib/System/Delegate.h"

#include "icalls/mscorlib/System.Diagnostics/Assert.h"
#include "icalls/mscorlib/System.Diagnostics/Debugger.h"
#include "icalls/mscorlib/System.Diagnostics/StackFrame.h"
#include "icalls/mscorlib/System.Diagnostics/StackTrace.h"
#include "icalls/mscorlib/System/Double.h"

#include "icalls/mscorlib/System/Enum.h"
#include "icalls/mscorlib/System/Environment.h"
#include "icalls/mscorlib/System/Exception.h"
#include "icalls/mscorlib/System/GC.h"

#include "icalls/mscorlib/System.Globalization/CalendarData.h"
#include "icalls/mscorlib/System.Globalization/CompareInfo.h"
#include "icalls/mscorlib/System.Globalization/CultureData.h"
#include "icalls/mscorlib/System.Globalization/CultureInfo.h"
#include "icalls/mscorlib/System.Globalization/RegionInfo.h"
#include "icalls/mscorlib/System.IO/DriveInfo.h"
#include "icalls/mscorlib/System.IO/MonoIO.h"
#include "icalls/mscorlib/System.IO/Path.h"
#include "icalls/mscorlib/System/Math.h"
#include "icalls/mscorlib/System/MissingMemberException.h"
#include "icalls/mscorlib/System/MonoCustomAttrs.h"
#include "icalls/mscorlib/System/MonoEnumInfo.h"
#include "icalls/mscorlib/System/MonoType.h"
#include "icalls/mscorlib/System/Number.h"
#include "icalls/mscorlib/System/NumberFormatter.h"
#include "icalls/mscorlib/System/Object.h"

#include "icalls/mscorlib/System.Reflection/Assembly.h"
#include "icalls/mscorlib/System.Reflection/AssemblyName.h"
#include "icalls/mscorlib/System.Reflection/CustomAttributeData.h"

#include "icalls/mscorlib/System.Reflection.Emit/AssemblyBuilder.h"
#include "icalls/mscorlib/System.Reflection.Emit/CustomAttributeBuilder.h"
#include "icalls/mscorlib/System.Reflection.Emit/DerivedType.h"
#include "icalls/mscorlib/System.Reflection.Emit/DynamicMethod.h"
#include "icalls/mscorlib/System.Reflection.Emit/EnumBuilder.h"
#include "icalls/mscorlib/System.Reflection.Emit/GenericTypeParameterBuilder.h"
#include "icalls/mscorlib/System.Reflection.Emit/MethodBuilder.h"
#include "icalls/mscorlib/System.Reflection.Emit/ModuleBuilder.h"
#include "icalls/mscorlib/System.Reflection.Emit/SignatureHelper.h"
#include "icalls/mscorlib/System.Reflection.Emit/SymbolType.h"
#include "icalls/mscorlib/System.Reflection.Emit/TypeBuilder.h"

#include "icalls/mscorlib/System.Reflection/FieldInfo.h"
#include "icalls/mscorlib/System.Reflection/MemberInfo.h"
#include "icalls/mscorlib/System.Reflection/MethodBase.h"
#include "icalls/mscorlib/System.Reflection/Module.h"
#include "icalls/mscorlib/System.Reflection/MonoCMethod.h"
#include "icalls/mscorlib/System.Reflection/MonoEventInfo.h"
#include "icalls/mscorlib/System.Reflection/MonoField.h"
#include "icalls/mscorlib/System.Reflection/MonoGenericClass.h"
#include "icalls/mscorlib/System.Reflection/MonoGenericCMethod.h"
#include "icalls/mscorlib/System.Reflection/MonoGenericMethod.h"
#include "icalls/mscorlib/System.Reflection/MonoMethod.h"
#include "icalls/mscorlib/System.Reflection/MonoMethodInfo.h"
#include "icalls/mscorlib/System.Reflection/MonoPropertyInfo.h"
#include "icalls/mscorlib/System.Reflection/ParameterInfo.h"
#include "icalls/mscorlib/System.Reflection/RtFieldInfo.h"

#include "icalls/mscorlib/System.Runtime.CompilerServices/RuntimeHelpers.h"

#include "icalls/mscorlib/System.Runtime.InteropServices/GCHandle.h"
#include "icalls/mscorlib/System.Runtime.InteropServices/Marshal.h"
#include "icalls/mscorlib/System.Runtime.InteropServices.WindowsRuntime/UnsafeNativeMethods.h"

#include "icalls/mscorlib/System.Runtime.Remoting.Activation/ActivationServices.h"
#include "icalls/mscorlib/System.Runtime.Remoting.Contexts/Context.h"
#include "icalls/mscorlib/System.Runtime.Remoting.Messaging/AsyncResult.h"
#include "icalls/mscorlib/System.Runtime.Remoting.Messaging/MonoMethodMessage.h"
#include "icalls/mscorlib/System.Runtime.Remoting.Proxies/RealProxy.h"
#include "icalls/mscorlib/System.Runtime.Remoting/RemotingServices.h"
#include "icalls/mscorlib/System.Runtime.Versioning/VersioningHelper.h"

#include "icalls/mscorlib/System/RuntimeFieldHandle.h"
#include "icalls/mscorlib/System/RuntimeMethodHandle.h"
#include "icalls/mscorlib/System/RuntimeTypeHandle.h"
#include "icalls/mscorlib/System/RuntimeType.h"

#include "icalls/mscorlib/System.Security.Cryptography/RNGCryptoServiceProvider.h"

#include "icalls/mscorlib/System.Security.Policy/Evidence.h"

#include "icalls/mscorlib/System.Security.Principal/WindowsIdentity.h"

#include "icalls/mscorlib/System.Security.Principal/WindowsImpersonationContext.h"
#include "icalls/mscorlib/System.Security.Principal/WindowsPrincipal.h"
#include "icalls/mscorlib/System.Security/SecurityFrame.h"

#include "icalls/mscorlib/System.Security/SecurityManager.h"
#include "icalls/mscorlib/System/SizedReference.h"
#include "icalls/mscorlib/System/String.h"

#include "icalls/mscorlib/System.Text/Encoding.h"
#include "icalls/mscorlib/System.Text/EncodingHelper.h"
#include "icalls/mscorlib/System.Text/Normalization.h"

#include "icalls/mscorlib/System.Threading/Interlocked.h"
#include "icalls/mscorlib/System.Threading/InternalThread.h"
#include "icalls/mscorlib/System.Threading/Monitor.h"
#include "icalls/mscorlib/System.Threading/Mutex.h"
#include "icalls/mscorlib/System.Threading/NativeEventCalls.h"
#include "icalls/mscorlib/System.Threading/Timer.h"
#include "icalls/mscorlib/System.Threading/Thread.h"
#include "icalls/mscorlib/System.Threading/ThreadPool.h"
#include "icalls/mscorlib/System.Threading/WaitHandle.h"
#include "icalls/mscorlib/System/TimeSpan.h"
#include "icalls/mscorlib/System/TimeZoneInfo.h"
#include "icalls/mscorlib/System/Type.h"
#include "icalls/mscorlib/System/TypedReference.h"
#include "icalls/mscorlib/System/ValueType.h"

#include "icalls/System/Microsoft.Win32/NativeMethods.h"
#include "icalls/System/Mono.Net.Security/MonoTlsProviderFactory.h"

#include "icalls/System.Configuration/System.Configuration/InternalConfigurationHost.h"
#include "icalls/System.Core/System.IO.MemoryMappedFiles/MemoryMapImpl.h"

#include "icalls/System/System.ComponentModel/Win32Exception.h"
#include "icalls/System/System.Configuration/DefaultConfig.h"
#include "icalls/System/System.Configuration/InternalConfigurationHost.h"

#include "icalls/System/System.IO/FAMWatcher.h"
#include "icalls/System/System.IO/FileSystemWatcher.h"
#include "icalls/System/System.IO/InotifyWatcher.h"
#include "icalls/System/System.IO/KqueueMonitor.h"
#include "icalls/System/System/IOSelector.h"

#include "icalls/System/System.Diagnostics/DefaultTraceListener.h"
#include "icalls/System/System.Diagnostics/FileVersionInfo.h"
#include "icalls/System/System.Diagnostics/PerformanceCounter.h"
#include "icalls/System/System.Diagnostics/PerformanceCounterCategory.h"
#include "icalls/System/System.Diagnostics/Process.h"
#include "icalls/System/System.Diagnostics/Stopwatch.h"

#include "icalls/System/System.Net/Dns.h"

#include "icalls/System/System.Net.NetworkInformation/LinuxNetworkInterface.h"
#include "icalls/System/System.Net.NetworkInformation/MacOsIPInterfaceProperties.h"
#include "icalls/System/System.Net.Sockets/Socket.h"
#include "icalls/System/System.Net.Sockets/SocketException.h"

#include "icalls/System/System.Threading/Semaphore.h"

#include "mono/ThreadPool/threadpool-ms.h"
#include "mono/ThreadPool/threadpool-ms-io.h"

#include "icalls/mscorlib/Mono/SafeStringMarshal.h"
#include "icalls/mscorlib/Mono/RuntimeMarshal.h"
#include "icalls/mscorlib/Mono/RuntimeGPtrArrayHandle.h"
#include "icalls/mscorlib/Mono/RuntimeClassHandle.h"

#include "icalls/mscorlib/System.Reflection/EventInfo.h"
#include "icalls/mscorlib/System.Reflection/PropertyInfo.h"
