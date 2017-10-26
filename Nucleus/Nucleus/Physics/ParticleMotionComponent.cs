﻿using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Physics
{
    /// <summary>
    /// A Physics Engine component that deals with resolving the motion of particles
    /// </summary>
    public class ParticleMotionComponent : Unique, IPhysicsEngineComponent
    {
        #region Properties

        /// <summary>
        /// Private backing field for Particles property
        /// </summary>
        private IList<Particle> _Particles = new List<Particle>();

        /// <summary>
        /// The set of particles 
        /// </summary>
        public IList<Particle> Particles
        {
            get { return _Particles; }
        }

        /// <summary>
        /// Private backing field for Damping property
        /// </summary>
        private double _Damping = 0.1;

        /// <summary>
        /// The damping factor to be applied to particle
        /// motion.  This is the proportional reduction
        /// in velocity of each particle each cycle.
        /// </summary>
        public double Damping
        {
            get { return _Damping; }
            set { _Damping = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a ParticleMotionComponent, generating particles from the
        /// specified nodes.
        /// </summary>
        /// <param name="nodes"></param>
        public ParticleMotionComponent(NodeCollection nodes)
        {
            foreach (Node node in nodes)
            {
                if (!node.HasData<Particle>())
                {
                    node.Data.Add(new Particle(node));
                }
                Particles.Add(node.GetData<Particle>());
            }
        }

        #endregion

        #region Methods

        public bool Cycle(double dt, PhysicsEngine engine)
        {
            double vFactor = 1.0 - Damping;
            foreach (var particle in Particles)
            {
                particle.Move(dt);
                particle.Damp(vFactor);
            }
            return true;
        }

        public bool Start(PhysicsEngine engine)
        {
            foreach (var particle in Particles) particle.Reset();
            return true;
        }

        #endregion
    }
}
