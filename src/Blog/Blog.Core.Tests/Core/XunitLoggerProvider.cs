using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Blog.Core.Tests.Core
{
    public class XunitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _output;

        public XunitLoggerProvider(ITestOutputHelper output)
        {
            _output = output;
        }

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new XunitLogger(_output);
        }

        private class XunitLogger : ILogger
        {
            private readonly ITestOutputHelper _output;

            private string Name { get; }

            public XunitLogger(ITestOutputHelper output)
            {
                _output = output;
                Name = nameof(XunitLogger);
            }

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception exception,
                Func<TState, Exception, string> formatter)
            {
                if (!this.IsEnabled(logLevel))
                    return;

                if (formatter == null)
                    throw new ArgumentNullException(nameof(formatter));

                var message = formatter(state, exception);
                if (string.IsNullOrEmpty(message) && exception == null)
                    return;

                var line = $"{logLevel} | {this.Name} | {message}";

                _output.WriteLine(line);

                if (exception != null)
                    _output.WriteLine(exception.ToString());
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return new XunitScope();
            }
        }

        private class XunitScope : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
