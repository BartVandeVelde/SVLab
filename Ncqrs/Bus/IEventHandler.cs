﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ncqrs.Domain.Bus
{
    public interface IEventHandler
    {
        void Handle(IEvent eventMessage);
    }
}
