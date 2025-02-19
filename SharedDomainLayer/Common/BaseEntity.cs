﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDomain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get;protected set; }

        protected BaseEntity() { 
            Id= Guid.NewGuid();
        }
    }
}
